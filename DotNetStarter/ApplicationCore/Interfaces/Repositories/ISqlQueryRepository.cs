using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface ISqlQueryRepository<T> where T : class
    {
        List<T> GetData(string query, params object[] parameters);

        List<CT> GetData<CT>(string query, params object[] parameters) where CT : class;

        int GetIntData(string query);

        decimal GetDecimalData(string query);

        List<dynamic> GetDynamicData(string query, params SqlParameter[] parameters);

        string GetStringValue(string query);

        Task<string> GetStringValueAsync(string query);

        int RunSqlCommand(string query, params object[] parameters);

        Task<int> RunSqlCommandAsync(string query, params object[] parameters);
    }
}
