using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using Autofac;
using Autofac.Extras.NLog;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Swashbuckle.Application;
using Microsoft.Owin.Security.OAuth;
using Presentation.Extends.DI;
using Presentation.Extends.Handlers;
using Presentation.Extends.Identity;
using Presentation.Services;
using Presentation.Extends.Helpers;
using EPYSLACSCustomer.Infrastructure.Data;
using EPYSLACSCustomer.Infrastructure.Data.Repositories;
using EPYSLACSCustomer.Core.Interfaces.Repositories;
using EPYSLACSCustomer.Core.Entities;
using EPYSLACSCustomer.Infrastructure.Services;
using EPYSLACSCustomer.Service.Reporting;

[assembly: OwinStartup(typeof(Presentation.Startup))]

namespace Presentation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            #region Web Api Config
            config.Services.Replace(typeof(IExceptionHandler), new ApiGlobalExceptionHandler());

            //var corsAttribute = new EnableCorsAttribute(ConfigurationManager.AppSettings[AppConstants.CorsOriginsSettingKey], "*", "*");
            //config.EnableCors(corsAttribute);

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            #endregion

            #region Configure Dependencies
            var builder = new ContainerBuilder();

            builder.RegisterType<AppDbContext>()
                   .AsSelf()
                   .InstancePerRequest();

            builder.Register(c => new UserStore<LoginUser, Role, int, UserLogin, UserRole, UserClaim>(new AppDbContext())).AsImplementedInterfaces();
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = new DpapiDataProtectionProvider("ApplicationName")
            });
            builder.Register(c => new DpapiDataProtectionProvider("App").Create()).As<IDataProtector>().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication);
            builder.RegisterType<EmailService>().AsSelf().InstancePerRequest();
            builder.RegisterType<SmsService>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>();
            builder.RegisterType<ApplicationSignInManager>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterGeneric(typeof(EfRepository<>))
                .As(typeof(IEfRepository<>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(EfRepository<>))
                .As(typeof(IEfRepository<>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(SqlQueryRepository<>))
                .As(typeof(ISqlQueryRepository<>))
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(SqlQueryRepository<>))
                .As(typeof(ISqlQueryRepository<>))
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(UserDTORepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(TSLLabellingService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<UserDTORepository>().PropertiesAutowired();
            
            builder.RegisterType<CommonHelpers>().AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<RDLReportDocument>().AsSelf().InstancePerRequest();

            builder.RegisterModule<NLogModule>();

            // Validators
            builder.RegisterModule<ValidationModule>();
            builder.RegisterModule<ValidationModuleWebApi>();

            // Automapper
            builder.RegisterModule<AutoMapperModule>();

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            // Register filters
            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(container, false);

            app.UseAutofacMiddleware(container);
            #endregion

            ConfigureAuth(app);

            #region Swagger Config
            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
                .EnableSwaggerUi();
            #endregion

            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}
