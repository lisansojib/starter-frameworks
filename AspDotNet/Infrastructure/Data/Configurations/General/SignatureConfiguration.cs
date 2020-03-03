using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class SignatureConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Signature>
    {
        public SignatureConfiguration()
            : this("dbo")
        {
        }

        public SignatureConfiguration(string schema)
        {
            ToTable("Signature", schema);
            HasKey(x => new { x.Field, x.Dates, x.CompanyId, x.SiteId });

            Ignore(x => x.Id);

            Property(x => x.Field).HasColumnName(@"Field").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Dates).HasColumnName(@"Dates").HasColumnType("datetime").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.LastNumber).HasColumnName(@"LastNumber").HasColumnType("decimal").IsRequired().HasPrecision(10,0);
            Property(x => x.CompanyId).HasColumnName(@"CompanyID").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(20).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.SiteId).HasColumnName(@"SiteID").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(10).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
    }

}

