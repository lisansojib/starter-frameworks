using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class MenuParamConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<MenuParam>
    {
        public MenuParamConfiguration()
            : this("dbo")
        {
        }

        public MenuParamConfiguration(string schema)
        {
            ToTable("MenuParam", schema);
            HasKey(x => new { x.Id, x.SeqNo });

            Property(x => x.Id).HasColumnName(@"MenuID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ParamType).HasColumnName(@"ParamType").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            Property(x => x.ParamValue).HasColumnName(@"ParamValue").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            Property(x => x.SeqNo).HasColumnName(@"SeqNo").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.Menu).WithMany(b => b.MenuParams).HasForeignKey(c => c.Id).WillCascadeOnDelete(false); // FK_MenuParam_Menu
        }
    }

}

