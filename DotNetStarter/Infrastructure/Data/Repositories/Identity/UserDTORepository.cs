using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using System.Linq;

namespace Infrastructure.Data.Repositories
{
    public class UserDTORepository : SqlQueryRepository<UserDto>, IUserDTORepository
    {
        public UserDTORepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public bool IsValidLogin(string username, string password)
        {
            var sql = $@"Select LU.UserCode
                From LoginUser LU
                INNER JOIN CompanyEntity CE On CE.CompanyID = LU.CompanyID
                INNER JOIN Employee E On E.EmployeeCode = LU.EmployeeCode
                INNER JOIN Application A On A.ApplicationID =  LU.DefaultApplicationID
                Where LU.IsSuperUser = 1 OR (LU.UserName = '{username}' And LU.Password = '{password}' And LU.IsActive = 1 And E.IsCustomer = 1)";

            var user = GetIntData(sql);
            return user > 0;
        }

        public UserDto GetLoginUser(int userCode)
        {
            var sql = $@"Select LU.UserCode Id, LU.Name, LU.Password, LU.EmailPassword, LU.IsSuperUser, LU.IsAdmin, LU.IsActive, LU.EmployeeCode, LU.CompanyID, LU.DefaultApplicationID, LU.UserTypeID
	                , CE.CompanyName, CompanyShortName=CE.ShortName, E.EmployeeName, ISNULL(C.ContactID, 0) ContactID, ISNULL(C.Name, '') [ContactName]
                From LoginUser LU
                Inner Join CompanyEntity CE On CE.CompanyID = LU.CompanyID
                Inner Join Employee E On E.EmployeeCode = LU.EmployeeCode
                Inner Join Application A On A.ApplicationID =  LU.DefaultApplicationID
                Left Join Contacts C On C.ContactID = E.ContactID
                Where LU.UserCode = {userCode}";

            var user = GetData(sql).FirstOrDefault();
            return user;
        }
    }
}
