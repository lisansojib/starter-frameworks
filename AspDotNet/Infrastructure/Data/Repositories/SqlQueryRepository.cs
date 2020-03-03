using ApplicationCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class SqlQueryRepository<T> : ISqlQueryRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;

        public SqlQueryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<T> GetData(string query, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<T>(query, parameters).ToList();
        }

        public List<CT> GetData<CT>(string query, params object[] parameters) where CT : class
        {
            return _dbContext.Database.SqlQuery<CT>(query, parameters).ToList();
        }

        public int GetIntData(string query)
        {
            return _dbContext.Database.SqlQuery<int>(query).FirstOrDefault();
        }

        public decimal GetDecimalData(string query)
        {
            return _dbContext.Database.SqlQuery<decimal>(query).FirstOrDefault();
        }

        public List<dynamic> GetDynamicData(string query, params SqlParameter[] parameters)
        {
            List<dynamic> dynamicList = new List<dynamic>();
            var command = _dbContext.Database.Connection.CreateCommand();
            command.CommandText = query;

            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.ParameterName;
                parameter.Value = param.Value;
                command.Parameters.Add(parameter);
            }

            try
            {
                _dbContext.Database.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < reader.FieldCount; fieldCount++)
                            row.Add(reader.GetName(fieldCount), reader[fieldCount]);

                        dynamicList.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }

            return dynamicList;
        }

        public string GetStringValue(string query)
        {
            return _dbContext.Database.SqlQuery<string>(query).FirstOrDefault();
        }

        public async Task<string> GetStringValueAsync(string query)
        {
            return await _dbContext.Database.SqlQuery<string>(query).FirstOrDefaultAsync();
        }

        public int RunSqlCommand(string query, params object[] parameters)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = _dbContext.Database.ExecuteSqlCommand(query, parameters);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return result;
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> RunSqlCommandAsync(string query, params object[] parameters)
        {
            using(var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = await _dbContext.Database.ExecuteSqlCommandAsync(query, parameters);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return result;
                }
                catch (System.Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
