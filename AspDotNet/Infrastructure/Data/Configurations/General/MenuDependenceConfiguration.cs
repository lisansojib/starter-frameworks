using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class MenuDependenceConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MenuDependence>
    {
        public MenuDependenceConfiguration()
            : this("dbo")
        {
        }

        public MenuDependenceConfiguration(string schema)
        {
            ToTable("MenuDependence", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"DependentID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.MenuId).HasColumnName(@"MenuID").HasColumnType("int").IsRequired();
            Property(x => x.DependentMenuId).HasColumnName(@"DependentMenuID").HasColumnType("int").IsRequired();
            Property(x => x.SeqNo).HasColumnName(@"SeqNo").HasColumnType("int").IsRequired();
            Property(x => x.RefNo).HasColumnName(@"RefNo").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(20);
            Property(x => x.UserWise).HasColumnName(@"UserWise").HasColumnType("bit").IsRequired();
            Property(x => x.OtherMenuId).HasColumnName(@"OtherMenuID").HasColumnType("int").IsRequired();
            Property(x => x.OtherDependentMenuId).HasColumnName(@"OtherDependentMenuID").HasColumnType("int").IsRequired();
            Property(x => x.MenuType).HasColumnName(@"MenuType").HasColumnType("bit").IsRequired();

            // Foreign keys
            HasRequired(a => a.DependentMenu).WithMany(b => b.MenuDependences_DependentMenuId).HasForeignKey(c => c.DependentMenuId).WillCascadeOnDelete(false); // FK_MenuDependence_Menu1
            HasRequired(a => a.Menu_MenuId).WithMany(b => b.MenuDependences_MenuId).HasForeignKey(c => c.MenuId).WillCascadeOnDelete(false); // FK_MenuDependence_Menu
        }
    }

}

