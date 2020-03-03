using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class SecurityRuleMenuConfiguration : EntityTypeConfiguration<SecurityRuleMenu>
    {
        public SecurityRuleMenuConfiguration()
            : this("dbo")
        {
        }

        public SecurityRuleMenuConfiguration(string schema)
        {
            ToTable("SecurityRuleMenu", schema);
            HasKey(x => new { x.MenuId, x.SecurityRuleCode });

            Property(x => x.MenuId).HasColumnName(@"MenuID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SecurityRuleCode).HasColumnName(@"SecurityRuleCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.CanSelect).HasColumnName(@"CanSelect").HasColumnType("bit").IsOptional();
            Property(x => x.CanInsert).HasColumnName(@"CanInsert").HasColumnType("bit").IsOptional();
            Property(x => x.CanUpdate).HasColumnName(@"CanUpdate").HasColumnType("bit").IsOptional();
            Property(x => x.CanDelete).HasColumnName(@"CanDelete").HasColumnType("bit").IsOptional();

            // Foreign keys
            HasRequired(a => a.Menu).WithMany(b => b.SecurityRuleMenus).HasForeignKey(c => c.MenuId).WillCascadeOnDelete(false); // FK_SecurityRule_Object_Menu
            HasRequired(a => a.SecurityRule).WithMany(b => b.SecurityRuleMenus).HasForeignKey(c => c.SecurityRuleCode).WillCascadeOnDelete(false); // FK_SecurityRuleMenu_SecurityRule
        }
    }
}
