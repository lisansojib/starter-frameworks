using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Statics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class ReportSuiteRepository : SqlQueryRepository<ReportSuite>, IReportSuiteRepository
    {
        public ReportSuiteRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<ReportSuiteMenuDTO>> GetMenusAsync(int userId, int applicationId, int companyId)
        {
            var menuList = new List<ReportSuiteMenuDTO>();
            var parentList = new List<ReportSuiteMenuDTO>();

            var command = _dbContext.Database.Connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[spGetReportMenuListForApi]";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@UserCode";
            parameter.Value = userId;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "@ApplicationID";
            parameter.Value = applicationId;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "@CompanyID";
            parameter.Value = companyId;
            command.Parameters.Add(parameter);

            try
            {
                _dbContext.Database.Connection.Open();
                var reader = await command.ExecuteReaderAsync();

                var objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;

                var records = objectContext.Translate<ReportSuiteMenuDTO>(reader, "MenuSet", MergeOption.AppendOnly).ToList();

                parentList = records.FindAll(x => x.ReportId == x.Parent_Key);
                menuList = records.FindAll(x => x.ReportId != x.Parent_Key);
                PopulateMenus(ref parentList, menuList);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }

            return parentList;
        }

        public async Task<DataSet> LoadReportParameterInfoAsync(int reportId)
        {
            var ds = new DataSet();
            var command = _dbContext.Database.Connection.CreateCommand();
            command.CommandText = $@"Declare @SQL As Varchar(8000)
	                                Set @SQL = (Select REPORT_SQL From ReportSuite Where ReportID = {reportId})
	                                IF(@SQL Is Null Or @SQL = '')
		                            Set @SQL = 'Select Null As Dummy Where 1 = 2'
	                                Exec (@SQL)";

            try
            {
                _dbContext.Database.Connection.Open();
                var reader = await command.ExecuteReaderAsync();
                ds = ExtensionMethods.DataReaderToDataSet(reader);
                return ds;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }
        }

        public DataSet LoadReportSourceDataSet(CommandType cmdType, string strCmdText, IDbDataParameter[] sqlParam)
        {
            var ds = new DataSet();
            IDataReader reader;
            IDbDataParameter parameter;

            try
            {
                var command = _dbContext.Database.Connection.CreateCommand();
                command.CommandType = cmdType;
                command.CommandText = strCmdText;
                foreach (var param in sqlParam)
                {
                    parameter = command.CreateParameter();
                    parameter.ParameterName = $"{param.ParameterName}";
                    parameter.Value = param.Value;
                    command.Parameters.Add(parameter);
                }

                _dbContext.Database.Connection.Open();
                reader = command.ExecuteReader();

                ds = ExtensionMethods.DataReaderToDataSet(reader);
                return ds;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }
        }

        public async Task<DataSet> LoadReportSourceDataSetAsync(CommandType cmdType, string strCmdText, IDbDataParameter[] sqlParam)
        {
            var ds = new DataSet();
            IDataReader reader;
            IDbDataParameter parameter;

            try
            {
                var command = _dbContext.Database.Connection.CreateCommand();
                command.CommandType = cmdType;
                foreach(var param in sqlParam)
                {
                    parameter = command.CreateParameter();
                    parameter.ParameterName = $"@{param.ParameterName}";
                    parameter.Value = param.Value;
                    command.Parameters.Add(parameter);
                }

                reader = await command.ExecuteReaderAsync();

                ds = ExtensionMethods.DataReaderToDataSet(reader);
                return ds;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }
        }

        #region Helpers
        private void PopulateMenus(ref List<ReportSuiteMenuDTO> parentList, List<ReportSuiteMenuDTO> menuList)
        {
            foreach (var parentMenu in parentList)
            {
                parentMenu.Childs = menuList.FindAll(x =>x.ReportId != parentMenu.Parent_Key && x.Parent_Key == parentMenu.ReportId).OrderBy(x => x.SeqNo).ToList();

                var subParents = parentMenu.Childs.FindAll(x => string.IsNullOrEmpty(x.Report_Name));
                foreach (var item in subParents)
                    item.Childs = PopulateChildMenu(menuList, item.ReportId);
            }                
        }

        private List<ReportSuiteMenuDTO> PopulateChildMenu(List<ReportSuiteMenuDTO> menuList, int parentId)
        {
            var childList = menuList.FindAll(x => x.Parent_Key == parentId).OrderBy(x => x.SeqNo).ToList();

            var subParents = childList.FindAll(x => string.IsNullOrEmpty(x.Report_Name));
            foreach (var childMenu in subParents)
                childMenu.Childs = PopulateChildMenu(menuList, childMenu.ReportId);

            return childList;
        }
        #endregion
    }
}
