using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class Select2Service : ISelect2Service
    {
        private readonly ISqlQueryRepository<Select2Option> _sqlQueryRepository;

        public Select2Service(ISqlQueryRepository<Select2Option> sqlQueryRepository)
        {
            _sqlQueryRepository = sqlQueryRepository;
        }

        public List<Select2Option> GetEntityTypeValues(int entityTypeId)
        {
            var query = $@"Select CAST(ValueID as varchar) [id], ValueName [text] 
                From EntityTypeValue
                Where EntityTypeID = {entityTypeId}
                Order By ValueName";

            var records = _sqlQueryRepository.GetData(query);
            return records;
        }
    }
}
