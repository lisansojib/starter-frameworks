using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class SecurityRuleFaqConfiguration : EntityTypeConfiguration<SecurityRuleFaq>
    {
        public SecurityRuleFaqConfiguration()
            : this("dbo")
        {
        }

        public SecurityRuleFaqConfiguration(string schema)
        {
            ToTable("SecurityRuleFAQ", schema);
            HasKey(x => new { x.FaqMasterId, x.SecurityRuleCode });

            Property(x => x.FaqMasterId).HasColumnName(@"FAQMasterID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SecurityRuleCode).HasColumnName(@"SecurityRuleCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.SecurityRule).WithMany(b => b.SecurityRuleFaqs).HasForeignKey(c => c.SecurityRuleCode).WillCascadeOnDelete(false);
        }
    }
}
