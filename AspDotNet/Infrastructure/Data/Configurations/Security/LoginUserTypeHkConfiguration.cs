using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class LoginUserTypeHkConfiguration : EntityTypeConfiguration<LoginUserTypeHk>
    {
        public LoginUserTypeHkConfiguration()
            : this("dbo")
        {
        }

        public LoginUserTypeHkConfiguration(string schema)
        {
            ToTable("LoginUserTypeHk");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("UserTypeID").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.UserType).HasMaxLength(30).HasColumnType("varchar").IsRequired();

            HasMany(x => x.LoginUsers).WithOptional(x => x.LoginUserTypeHk).HasForeignKey(x => x.UserTypeId);
        }
    }
}
