using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class SecurityRuleReportConfiguration : EntityTypeConfiguration<SecurityRuleReport>
    {
        public SecurityRuleReportConfiguration()
            : this("dbo")
        {
        }

        public SecurityRuleReportConfiguration(string schema)
        {
            ToTable("SecurityRuleReport", schema);
            HasKey(x => new { x.ReportId, x.SecurityRuleCode });

            Property(x => x.ReportId).HasColumnName(@"ReportID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SecurityRuleCode).HasColumnName(@"SecurityRuleCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.ReportSuite).WithMany(b => b.SecurityRuleReports).HasForeignKey(c => c.ReportId).WillCascadeOnDelete(false); // FK_SecurityRuleReport_ReportSuite
            HasRequired(a => a.SecurityRule).WithMany(b => b.SecurityRuleReports).HasForeignKey(c => c.SecurityRuleCode).WillCascadeOnDelete(false); // FK_SecurityRuleReport_SecurityRule
        }
    }
}
