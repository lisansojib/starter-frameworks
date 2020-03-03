using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class LoginUserConfiguration : EntityTypeConfiguration<LoginUser>
    {
        public LoginUserConfiguration()
            : this("dbo")
        {
        }

        public LoginUserConfiguration(string schema)
        {
            ToTable("LoginUser");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("UserCode").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(100).HasColumnType("varchar");
            Property(x => x.EmailPassword).HasMaxLength(20).HasColumnType("varchar");
        }
    }
}
