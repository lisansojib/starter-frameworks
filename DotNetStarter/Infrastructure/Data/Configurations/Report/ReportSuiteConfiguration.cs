using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class ReportSuiteConfiguration : EntityTypeConfiguration<ReportSuite>
    {
        public ReportSuiteConfiguration()
            : this("dbo")
        {
        }

        public ReportSuiteConfiguration(string schema)
        {
            ToTable("ReportSuite", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"REPORTID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ParentKey).HasColumnName(@"PARENT_KEY").HasColumnType("int").IsOptional();
            Property(x => x.NodeText).HasColumnName(@"NODE_TEXT").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(200);
            Property(x => x.ReportName).HasColumnName(@"REPORT_NAME").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);
            Property(x => x.ReportPathName).HasColumnName(@"REPORT_PATH_NAME").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(255);
            Property(x => x.ReportSql).HasColumnName(@"REPORT_SQL").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(8000);
            Property(x => x.SeqNo).HasColumnName(@"SeqNo").HasColumnType("int").IsRequired();
            Property(x => x.IsVisible).HasColumnName(@"IsVisible").HasColumnType("bit").IsRequired();
            Property(x => x.HasDefaultValue).HasColumnName(@"HasDefaultValue").HasColumnType("bit").IsRequired();
            Property(x => x.IsMultipleSelection).HasColumnName(@"IsMultipleSelection").HasColumnType("bit").IsRequired();
            Property(x => x.ApplicationId).HasColumnName(@"ApplicationID").HasColumnType("int").IsRequired();
            Property(x => x.HasExternalReport).HasColumnName(@"HasExternalReport").HasColumnType("bit").IsRequired();
            Property(x => x.IsApi).HasColumnName(@"IsApi").HasColumnType("bit").IsRequired();
        }
    }
}
