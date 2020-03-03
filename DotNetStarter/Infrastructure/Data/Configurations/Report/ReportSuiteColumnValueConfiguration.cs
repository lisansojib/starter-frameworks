using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class ReportSuiteColumnValueConfiguration : EntityTypeConfiguration<ReportSuiteColumnValue>
    {
        public ReportSuiteColumnValueConfiguration()
            : this("dbo")
        {
        }

        public ReportSuiteColumnValueConfiguration(string schema)
        {
            ToTable("ReportSuiteColumnValue", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"ReportSuiteColumnID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ReportId).HasColumnName(@"ReportID").HasColumnType("int").IsRequired();
            Property(x => x.ColumnId).HasColumnName(@"ColumnID").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(100);
            Property(x => x.DefaultValue).HasColumnName(@"DefaultValue").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.Source).HasColumnName(@"Source").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(150);
            Property(x => x.DaoClass).HasColumnName(@"DAOClass").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(150);
            Property(x => x.MethodName).HasColumnName(@"MethodName").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(150);
            Property(x => x.ParameterName).HasColumnName(@"ParameterName").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(200);
            Property(x => x.IsHidden).HasColumnName(@"IsHidden").HasColumnType("bit").IsRequired();
            Property(x => x.ParentColumns).HasColumnName(@"ParentColumns").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(200);
            Property(x => x.ValueColumnId).HasColumnName(@"ValueColumnID").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.DisplayColumnId).HasColumnName(@"DisplayColumnID").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.IsMultipleSelection).HasColumnName(@"IsMultipleSelection").HasColumnType("bit").IsRequired();
            Property(x => x.DefaultValueDaoClass).HasColumnName(@"DefaultValueDAOClass").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(150);
            Property(x => x.DefaultValueMethodName).HasColumnName(@"DefaultValueMethodName").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(150);
            Property(x => x.DefaultValueColumnId).HasColumnName(@"DefaultValueColumnID").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.ShowAdditionalColumn).HasColumnName(@"ShowAdditionalColumn").HasColumnType("bit").IsRequired();
            Property(x => x.AdditionalColumnId).HasColumnName(@"AdditionalColumnID").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(150);
            Property(x => x.AdditionalColumnHeader).HasColumnName(@"AdditionalColumnHeader").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(200);
            Property(x => x.ColumnWidth).HasColumnName(@"ColumnWidth").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(200);

            // Foreign keys
            HasRequired(a => a.ReportSuite).WithMany(b => b.ReportSuiteColumnValues).HasForeignKey(c => c.ReportId).WillCascadeOnDelete(false); // FK_ReportSuiteColumnValue_ReportSuite
        }
    }
}
