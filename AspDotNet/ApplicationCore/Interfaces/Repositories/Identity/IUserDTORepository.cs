using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces.Repositories
{
    public interface IUserDTORepository : ISqlQueryRepository<UserDto>
    {
        bool IsValidLogin(string username, string password);
        UserDto GetLoginUser(int userCode);
    }
}
