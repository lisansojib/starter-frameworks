using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using ApplicationCore.Entities;
using Presentation.Services;

namespace Presentation.Extends.Identity
{
    public class ApplicationUserManager : UserManager<LoginUser, int>
    {
        public ApplicationUserManager(IUserStore<LoginUser, int> store, EmailService emailService, SmsService smsService, IdentityFactoryOptions<ApplicationUserManager> options) : base(store)
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<LoginUser, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<LoginUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<LoginUser, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = emailService;
            SmsService = smsService;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");

                UserTokenProvider = new DataProtectorTokenProvider<LoginUser, int>(dataProtector);
            }

            //alternatively use this if you are running in Azure Web Sites
            UserTokenProvider = new EmailTokenProvider<LoginUser, int>();
        }
    }
}