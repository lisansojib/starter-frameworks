using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Infrastructure.Data.Repositories
{
    public class MenuRepository : SqlQueryRepository<Menu>, IMenuRepository
    {
        public MenuRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public List<MenuDTO> GetMenus(int userId, int applicationId, int companyId)
        {
            var menuList = new List<MenuDTO>();

            var command = _dbContext.Database.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "[dbo].[spGetMenuList]";

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
                var reader = command.ExecuteReader();

                var objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;

                menuList = objectContext.Translate<MenuDTO>(reader, "MenuSet", MergeOption.AppendOnly).ToList();

                reader.NextResult();

                var childs = objectContext.Translate<MenuDTO>(reader, "MenuSet", MergeOption.AppendOnly).ToList();
                PopulateChilds(ref menuList, ref childs);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }

            return menuList;
        }

        #region Helpers
        private void PopulateChilds(ref List<MenuDTO> menuList, ref List<MenuDTO> childs)
        {
            foreach(var item in menuList)
            {
                item.Childs = childs.Where(x => x.ParentId == item.MenuId).ToList();
                var mList = item.Childs;

                if(mList.Any())
                    PopulateChilds(ref mList, ref childs);
            }
        }
        #endregion
    }
}
