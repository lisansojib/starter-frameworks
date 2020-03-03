using EPYSLACSCustomer.Core.DTOs;
using EPYSLACSCustomer.Core.Entities;
using EPYSLACSCustomer.Core.Interfaces.Repositories;
using Presentation.Extends.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private static UserDto _appUser;
        private readonly IEfRepository<Employee> _employeeRepository;

        public BaseController()
        {
            _userManager = DependencyResolver.Current.GetService<ApplicationUserManager>();
            _employeeRepository = DependencyResolver.Current.GetService<IEfRepository<Employee>>();
        }

        protected int UserId => User.Identity.GetUserId<int>();
        protected UserDto AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    var loginUser = _userManager.FindById(UserId);
                    if (loginUser == null) return null;

                    _appUser = new UserDto
                    {
                        Id = loginUser.Id,
                        Name = loginUser.Name,
                        Password = loginUser.Password,
                        EmailPassword = loginUser.EmailPassword,
                        IsSuperUser = loginUser.IsSuperUser,
                        IsAdmin = loginUser.IsAdmin,
                        IsActive = loginUser.IsActive,
                        EmployeeCode = loginUser.EmployeeCode ?? 0,
                        CompanyId = loginUser.CompanyId ?? 0,
                        UserTypeId = loginUser.UserTypeId ?? 0
                    };

                    var employee = _employeeRepository.Find(loginUser.EmployeeCode.Value);
                    _appUser.EmployeeName = employee?.EmployeeName;
                }

                return _appUser;
            }
            set
            {
                _appUser = value;
            }
        }

        protected string UserIp => Request.UserHostAddress;
        protected string BaseUrl => Request.Url.GetLeftPart(UriPartial.Authority);
    }
}