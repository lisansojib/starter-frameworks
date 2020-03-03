using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class GroupUserSecurityRuleConfiguration : EntityTypeConfiguration<GroupUserSecurityRule>
    {
        public GroupUserSecurityRuleConfiguration()
            : this("dbo")
        {
        }

        public GroupUserSecurityRuleConfiguration(string schema)
        {
            ToTable("GroupUserSecurityRule", schema);
            HasKey(x => new { x.GroupCode, x.SecurityRuleCode });

            Property(x => x.GroupCode).HasColumnName(@"GroupCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SecurityRuleCode).HasColumnName(@"SecurityRuleCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);            

            // Foreign keys
            HasRequired(a => a.GroupUser).WithMany(b => b.GroupUserSecurityRules).HasForeignKey(c => c.GroupCode).WillCascadeOnDelete(false); // FK_GroupUserSecurityRule_GroupUser
            HasRequired(a => a.SecurityRule).WithMany(b => b.GroupUserSecurityRules).HasForeignKey(c => c.SecurityRuleCode).WillCascadeOnDelete(false); // FK_GroupUserSecurityRule_SecurityRule
        }
    }
}
