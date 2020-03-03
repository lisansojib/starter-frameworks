using ApplicationCore.DTOs;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces.Services
{
    public interface ISelect2Service
    {
        List<Select2Option> GetEntityTypeValues(int entityTypeId);
    }
}
