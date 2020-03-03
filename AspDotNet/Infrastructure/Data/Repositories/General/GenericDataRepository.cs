using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Infrastructure.Data.Repositories
{
    public class GenericDataRepository : SqlQueryRepository<BaseDto>, IGenericDataRepository
    {
        public GenericDataRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        public List<BaseDto> GetOrderInformationPOForXLS(DateTime formDate, DateTime toDate, string buyers)
        {
            var list = new List<BaseDto>();

            var command = _dbContext.Database.Connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "[dbo].[spRPTOrderInformationPOforXLS]";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@FromDate";
            parameter.Value = formDate;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "@ToDate";
            parameter.Value = toDate;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.ParameterName = "@Buyer";
            parameter.Value = buyers;
            command.Parameters.Add(parameter);

            try
            {
                _dbContext.Database.Connection.Open();
                var reader = command.ExecuteReader();

                var objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
                list = objectContext.Translate<BaseDto>(reader, "BaseDto", MergeOption.AppendOnly).ToList();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                _dbContext.Database.Connection.Close();
            }

            return list;
        }
    }
}
