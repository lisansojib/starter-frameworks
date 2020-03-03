using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class SecurityRuleConfiguration : EntityTypeConfiguration<SecurityRule>
    {
        public SecurityRuleConfiguration()
            : this("dbo")
        {
        }

        public SecurityRuleConfiguration(string schema)
        {
            ToTable("SecurityRule", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"SecurityRuleCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SecurityRuleName).HasColumnName(@"SecurityRuleName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
            Property(x => x.SecurityRuleDescription).HasColumnName(@"SecurityRuleDescription").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(500);
        }
    }
}
