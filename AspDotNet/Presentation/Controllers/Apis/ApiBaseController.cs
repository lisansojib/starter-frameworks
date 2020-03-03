using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public abstract class ApiBaseController : ApiController
    {
        private readonly IUserDTORepository _userRepository;
        private static UserDto _appUser;

        public ApiBaseController()
        {
            _userRepository = DependencyResolver.Current.GetService<IUserDTORepository>();
        }

        protected int UserId => User.Identity.GetUserId<int>();
        protected UserDto AppUser
        {
            get
            {
                if(_appUser == null)
                {
                    _appUser = _userRepository.GetLoginUser(UserId);
                    if (_appUser == null)
                        throw new Exception("Can't not find logged in user.");
                }  
                
                return _appUser;
            }
            set
            {
                _appUser = value;
            }
        }
        protected string UserIp => HttpContext.Current.Request.UserHostAddress;
        protected string BaseUrl => Request.RequestUri.GetLeftPart(UriPartial.Authority);
    }
}
