using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class MenuConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Menu>
    {
        public MenuConfiguration()
            : this("dbo")
        {
        }

        public MenuConfiguration(string schema)
        {
            ToTable("Menu", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"MenuID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ApplicationId).HasColumnName(@"ApplicationID").HasColumnType("int").IsRequired();
            Property(x => x.ParentId).HasColumnName(@"ParentID").HasColumnType("int").IsRequired();
            Property(x => x.DockPanel).HasColumnName(@"DockPanel").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(50);
            Property(x => x.MenuCaption).HasColumnName(@"MenuCaption").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.PageName).HasColumnName(@"PageName").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.TabCaption).HasColumnName(@"TabCaption").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(100);
            Property(x => x.NavigateUrl).HasColumnName(@"NavigateUrl").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(250);
            Property(x => x.ImageUrl).HasColumnName(@"ImageUrl").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(250);
            Property(x => x.TabWidth).HasColumnName(@"TabWidth").HasColumnType("int").IsRequired();
            Property(x => x.PageWidth).HasColumnName(@"PageWidth").HasColumnType("int").IsRequired();
            Property(x => x.PageHeight).HasColumnName(@"PageHeight").HasColumnType("int").IsRequired();
            Property(x => x.SeqNo).HasColumnName(@"SeqNo").HasColumnType("int").IsRequired();
            Property(x => x.IsVisible).HasColumnName(@"IsVisible").HasColumnType("bit").IsRequired();
            Property(x => x.RestrictionLimit).HasColumnName(@"RestrictionLimit").HasColumnType("int").IsRequired();
            Property(x => x.IsAdminOnly).HasColumnName(@"IsAdminOnly").HasColumnType("bit").IsRequired();
            Property(x => x.SingleUserView).HasColumnName(@"SingleUserView").HasColumnType("bit").IsRequired();
            Property(x => x.HasAutoNo).HasColumnName(@"HasAutoNo").HasColumnType("bit").IsRequired();
            Property(x => x.UseCommonInterface).HasColumnName(@"UseCommonInterface").HasColumnType("bit").IsRequired();
            Property(x => x.ModuleSelection).HasColumnName(@"ModuleSelection").HasColumnType("int").IsRequired();
            Property(x => x.HasParam).HasColumnName(@"HasParam").HasColumnType("bit").IsRequired();
            Property(x => x.MLevel).HasColumnName(@"MLevel").HasColumnType("int").IsRequired();

            // Foreign keys
            HasRequired(a => a.Application).WithMany(b => b.Menus).HasForeignKey(c => c.ApplicationId).WillCascadeOnDelete(false); // FK_Menu_Application
        }
    }

}

