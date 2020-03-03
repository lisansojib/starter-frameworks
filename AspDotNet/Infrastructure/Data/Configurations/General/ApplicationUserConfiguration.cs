using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
            : this("dbo")
        {
        }

        public ApplicationUserConfiguration(string schema)
        {
            ToTable("ApplicationUser", schema);
            HasKey(x => new { x.ApplicationId, x.UserCode });

            Ignore(x => x.Id);
            Property(x => x.ApplicationId).HasColumnName(@"ApplicationID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.UserCode).HasColumnName(@"UserCode").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsOptional();

            // Foreign keys
            HasRequired(a => a.Application).WithMany(b => b.ApplicationUsers).HasForeignKey(c => c.ApplicationId); // FK_ApplicationUser_Application
        }
    }

}

