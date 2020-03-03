using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class LoginUserAttachedWithGroupUserConfiguration : EntityTypeConfiguration<LoginUserAttachedWithGroupUser>
    {
        public LoginUserAttachedWithGroupUserConfiguration()
            : this("dbo")
        {
        }

        public LoginUserAttachedWithGroupUserConfiguration(string schema)
        {
            ToTable("LoginUserAttachedWithGroupUser", schema);
            HasKey(x => new { x.UserCode, x.GroupCode });

            Property(x => x.UserCode).HasColumnName(@"UserCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.GroupCode).HasColumnName(@"GroupCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            // Foreign keys
            HasRequired(a => a.GroupUser).WithMany(b => b.LoginUserAttachedWithGroupUsers).HasForeignKey(c => c.GroupCode).WillCascadeOnDelete(false); // FK_LoginUserAttachedWithGroupUser_GroupUser
            HasRequired(a => a.LoginUser).WithMany(b => b.LoginUserAttachedWithGroupUsers).HasForeignKey(c => c.UserCode).WillCascadeOnDelete(false); // FK_LoginUserAttachedWithGroupUser_LoginUser
        }
    }
}
