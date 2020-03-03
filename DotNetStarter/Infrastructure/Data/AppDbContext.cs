using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ApplicationCore.Entities;
using ApplicationCore.Statics;
using Infrastructure.Data.Configurations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<LoginUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public AppDbContext()
            : base(Constants.CONNECTION_Name)
        {
        }

        #region General Entities
        public virtual IDbSet<Application> ApplicationSet { get; set; }
        public virtual IDbSet<ApplicationUser> ApplicationUserSet { get; set; }
        public virtual IDbSet<Company> CompanySet { get; set; }
        public virtual IDbSet<CompanyEntity> CompanyEntitySet { get; set; }
        public virtual IDbSet<Employee> EmployeeSet { get; set; }
        public virtual IDbSet<EmployeeDepartment> EmployeeDepartmentSet { get; set; }
        public virtual IDbSet<EmployeeDesignation> EmployeeDesignationSet { get; set; }
        public virtual IDbSet<EmployeeSection> EmployeeSectionSet { get; set; }
        public virtual IDbSet<EmployeeStatus> EmployeeStatusSet { get; set; }
        public virtual IDbSet<EmployeeSubSection> EmployeeSubSectionSet { get; set; }
        public virtual IDbSet<EntityType> EntityTypeSet { get; set; }
        public virtual IDbSet<EntityTypeValue> EntityTypeValueSet { get; set; }
        public virtual IDbSet<FinancialCalander> FinancialCalanderSet { get; set; }
        public virtual IDbSet<FinancialCalanderSetup> FinancialCalanderSetupSet { get; set; }
        public virtual IDbSet<FinancialYear> FinancialYearSet { get; set; }
        public virtual IDbSet<GroupUser> GroupUserSet { get; set; }
        public virtual IDbSet<Menu> MenuSet { get; set; }
        public virtual IDbSet<MenuDependence> MenuDependenceSet { get; set; }
        public virtual IDbSet<MenuParam> MenuParamSet { get; set; }
        public virtual IDbSet<MessageEvent> MessageEventSet { get; set; }
        public virtual IDbSet<MessageQueue> MessageQueueSet { get; set; }
        public virtual IDbSet<Signature> SignatureSet { get; set; }
        #endregion

        #region Labelling
        public virtual IDbSet<TSLLabellingMaster> TslLabellingMasterSet { get; set; }
        public virtual IDbSet<TSLLabellingChild> TslLabellingChildSet { get; set; }
        public virtual IDbSet<TSLLabellingHistory> TSLLabellingHistorySet { get; set; }
        public virtual IDbSet<UKAndCELabellingChild> UkAndCeLabellingChildSet { get; set; }
        public virtual IDbSet<UKAndCELabellingMaster> UkAndCeLabellingMasterSet { get; set; }
        public virtual IDbSet<UKAndCELabellingHistory> UKAndCELabellingHistorySet { get; set; }
        #endregion

        #region Report Entities
        public virtual IDbSet<ReportSuite> ReportSuiteSet { get; set; }
        public virtual IDbSet<ReportSuiteColumnValue> SuiteColumnValueSet { get; set; }
        public virtual IDbSet<ReportSuiteExternalSetup> SuiteExternalSetupSet { get; set; }
        #endregion

        #region Security
        public virtual IDbSet<GroupUserSecurityRule> GroupUserSecurityRuleSet { get; set; }
        public virtual IDbSet<LoginUserAttachedWithGroupUser> LoginUserAttachedWithGroupUserSet { get; set; }
        public virtual IDbSet<LoginUserTypeHk> LoginUserTypeHkSet { get; set; }
        public virtual IDbSet<SecurityRule> SecurityRuleSet { get; set; }
        public virtual IDbSet<SecurityRuleFaq> SecurityRuleFaqSet { get; set; }
        public virtual IDbSet<SecurityRuleFeature> SecurityRuleFeatureSet { get; set; }
        public virtual IDbSet<SecurityRuleMenu> SecurityRuleMenuSet { get; set; }
        public virtual IDbSet<SecurityRuleReport> SecurityRuleReportSet { get; set; }
        public virtual IDbSet<ClientMaster> ClientMasterSet { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new LoginUserConfiguration());
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");

            #region General Entities Configuration
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new CompanyEntityConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new EmployeeDepartmentConfiguration());
            modelBuilder.Configurations.Add(new EmployeeDesignationConfiguration());
            modelBuilder.Configurations.Add(new EmployeeSectionConfiguration());
            modelBuilder.Configurations.Add(new EmployeeStatusConfiguration());
            modelBuilder.Configurations.Add(new EmployeeSubSectionConfiguration());
            modelBuilder.Configurations.Add(new EntityTypeConfiguration());
            modelBuilder.Configurations.Add(new EntityTypeValueConfiguration());
            modelBuilder.Configurations.Add(new FinancialCalanderConfiguration());
            modelBuilder.Configurations.Add(new FinancialCalanderSetupConfiguration());
            modelBuilder.Configurations.Add(new FinancialYearConfiguration());
            modelBuilder.Configurations.Add(new MenuConfiguration());
            modelBuilder.Configurations.Add(new MenuDependenceConfiguration());
            modelBuilder.Configurations.Add(new MenuParamConfiguration());
            modelBuilder.Configurations.Add(new MessageEventConfiguration());
            modelBuilder.Configurations.Add(new MessageQueueConfiguration());
            modelBuilder.Configurations.Add(new SignatureConfiguration());
            #endregion

            #region Labelling
            modelBuilder.Configurations.Add(new TSLLabellingMasterConfiguration());
            modelBuilder.Configurations.Add(new TSLLabellingChildConfiguration());
            modelBuilder.Configurations.Add(new TSLLabellingHistoryConfiguration());
            modelBuilder.Configurations.Add(new UKAndCELabellingChildConfiguration());
            modelBuilder.Configurations.Add(new UKAndCELabellingMasterConfiguration());
            modelBuilder.Configurations.Add(new UKAndCELabellingHistoryConfiguration());
            #endregion

            #region Report Configuration
            modelBuilder.Configurations.Add(new ReportSuiteConfiguration());
            modelBuilder.Configurations.Add(new ReportSuiteColumnValueConfiguration());
            modelBuilder.Configurations.Add(new ReportSuiteExternalSetupConfiguration());
            #endregion

            #region Security Configuration
            modelBuilder.Configurations.Add(new GroupUserConfiguration());
            modelBuilder.Configurations.Add(new GroupUserSecurityRuleConfiguration());
            modelBuilder.Configurations.Add(new LoginUserAttachedWithGroupUserConfiguration());
            modelBuilder.Configurations.Add(new LoginUserTypeHkConfiguration());
            modelBuilder.Configurations.Add(new SecurityRuleConfiguration());
            modelBuilder.Configurations.Add(new SecurityRuleFaqConfiguration());
            modelBuilder.Configurations.Add(new SecurityRuleFeatureConfiguration());
            modelBuilder.Configurations.Add(new SecurityRuleMenuConfiguration());
            modelBuilder.Configurations.Add(new SecurityRuleReportConfiguration());
            modelBuilder.Configurations.Add(new ClientMasterConfiguration());
            #endregion
        }
    }
}
