using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class ReportSuiteExternalSetupConfiguration : EntityTypeConfiguration<ReportSuiteExternalSetup>
    {
        public ReportSuiteExternalSetupConfiguration()
            : this("dbo")
        {
        }

        public ReportSuiteExternalSetupConfiguration(string schema)
        {
            ToTable("ReportSuiteExternalSetup", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"SetupID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Reportid).HasColumnName(@"REPORTID").HasColumnType("int").IsRequired();
            Property(x => x.ExternalId).HasColumnName(@"ExternalID").HasColumnType("int").IsRequired();
            Property(x => x.ReportName).HasColumnName(@"REPORT_NAME").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);
            Property(x => x.ReportPathName).HasColumnName(@"REPORT_PATH_NAME").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);

            // Foreign keys
            HasRequired(a => a.ReportSuite).WithMany(b => b.ReportSuiteExternalSetups).HasForeignKey(c => c.Reportid).WillCascadeOnDelete(false);
        }
    }
}
