using ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class SecurityRuleFeatureConfiguration : EntityTypeConfiguration<SecurityRuleFeature>
    {
        public SecurityRuleFeatureConfiguration()
            : this("dbo")
        {
        }

        public SecurityRuleFeatureConfiguration(string schema)
        {
            ToTable("SecurityRuleFeature", schema);
            HasKey(x => new { x.FeatureId, x.SecurityRuleCode });

            Property(x => x.FeatureId).HasColumnName(@"FeatureID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.SecurityRuleCode).HasColumnName(@"SecurityRuleCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.SecurityRule).WithMany(b => b.SecurityRuleFeatures).HasForeignKey(c => c.SecurityRuleCode).WillCascadeOnDelete(false); // FK_SecurityRuleFeature_SecurityRule
        }
    }
}
