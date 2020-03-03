using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IMenuRepository : ISqlQueryRepository<Menu>
    {
        List<MenuDto> GetMenus(int userId, int applicationId, int companyId);
    }
}
