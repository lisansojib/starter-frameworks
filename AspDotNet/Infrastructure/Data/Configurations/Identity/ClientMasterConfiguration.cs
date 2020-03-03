using ApplicationCore.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Infrastructure.Data.Configurations
{
    public class ClientMasterConfiguration : EntityTypeConfiguration<ClientMaster>
    {
        public ClientMasterConfiguration()
            : this("dbo")
        {
        }

        public ClientMasterConfiguration(string schema)
        {
            ToTable("ClientMaster", schema);
            HasKey(x => x.Id);

            Property(x => x.ClientId).IsRequired().HasMaxLength(128);
            Property(x => x.ClientSecret).IsRequired().HasMaxLength(128);
            Property(x => x.ClientName).HasMaxLength(100).IsRequired();
        }
    }
}
