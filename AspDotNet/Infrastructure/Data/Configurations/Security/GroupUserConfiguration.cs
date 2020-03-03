using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class GroupUserConfiguration : EntityTypeConfiguration<GroupUser>
    {
        public GroupUserConfiguration()
            : this("dbo")
        {
        }

        public GroupUserConfiguration(string schema)
        {
            ToTable("GroupUser", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"GroupCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.GroupName).HasColumnName(@"GroupName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(250);
            Property(x => x.GroupDescription).HasColumnName(@"GroupDescription").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(500);
            Property(x => x.DefaultApplicationId).HasColumnName(@"DefaultApplicationID").HasColumnType("int").IsRequired();
        }
    }
}
