using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IReportSuiteRepository : ISqlQueryRepository<ReportSuite>
    {
        Task<List<ReportSuiteMenuDTO>> GetMenusAsync(int userId, int applicationId, int companyId);

        Task<DataSet> LoadReportParameterInfoAsync(int reportId);

        DataSet LoadReportSourceDataSet(CommandType cmdType, string strCmdText, IDbDataParameter[] sqlParam);

        Task<DataSet> LoadReportSourceDataSetAsync(CommandType cmdType, string strCmdText, IDbDataParameter[] sqlParam);
    }
}
