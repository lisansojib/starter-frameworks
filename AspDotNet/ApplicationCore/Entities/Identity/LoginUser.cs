using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class LoginUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public LoginUser()
        {
            EmployeeCode = 0;
            CompanyId = 0;
            UserTypeId = 0;
            AddedBy = 0;
            DateAdded = DateTime.Now;
            LoginUserAttachedWithGroupUsers = new List<LoginUserAttachedWithGroupUser>();
            ApplicationUsers = new List<ApplicationUser>();
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string EmailPassword { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime? LogonTime { get; set; }
        public int? EmployeeCode { get; set; }
        public int? CompanyId { get; set; }
        public int DefaultApplicationId { get; set; }
        public int? UserTypeId { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Child LoginUserAttachedWithGroupUsers where [LoginUserAttachedWithGroupUser].[UserCode] point to this entity (FK_LoginUserAttachedWithGroupUser_LoginUser)
        /// </summary>
        public virtual ICollection<LoginUserAttachedWithGroupUser> LoginUserAttachedWithGroupUsers { get; set; }

        /// <summary>
        /// Parent LoginUserTypeHk pointed by [LoginUser].([UserTypeId]) (FK_LoginUser_LoginUserType_HK)
        /// </summary>
        public virtual LoginUserTypeHk LoginUserTypeHk { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        #region Methods
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<LoginUser, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<LoginUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        #endregion
    }

    /// <summary>
    /// Role
    /// </summary>
    public class Role : IdentityRole<int, UserRole>
    {
    }

    /// <summary>
    /// User Role
    /// </summary>
    public class UserRole : IdentityUserRole<int>
    {
    }

    /// <summary>
    /// User Claim
    /// </summary>
    public class UserClaim : IdentityUserClaim<int>
    {
    }

    /// <summary>
    /// UserLogin
    /// </summary>
    public class UserLogin : IdentityUserLogin<int>
    {
    }
}
