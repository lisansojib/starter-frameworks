using ApplicationCore.DTOs;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IGenericDataRepository : ISqlQueryRepository<BaseDto>
    {
        List<BaseDto> GetOrderInformationPOForXLS(DateTime formDate, DateTime toDate, string buyers);
    }
}
