namespace Infrastructure.Data.Configurations
{
    public class ApplicationConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Core.Entities.Application>
    {
        public ApplicationConfiguration()
            : this("dbo")
        {
        }

        public ApplicationConfiguration(string schema)
        {
            ToTable("Application", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"ApplicationID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ApplicationName).HasColumnName(@"ApplicationName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            Property(x => x.ApplicationDescription).HasColumnName(@"ApplicationDescription").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(1000);
            Property(x => x.IsDefault).HasColumnName(@"IsDefault").HasColumnType("bit").IsRequired();
            Property(x => x.ApplicationLogo).HasColumnName(@"ApplicationLogo").HasColumnType("image").IsOptional().HasMaxLength(2147483647);
            Property(x => x.ApplicationLogoPath).HasColumnName(@"ApplicationLogoPath").HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(250);
            Property(x => x.SequenceNo).HasColumnName(@"SequenceNo").HasColumnType("int").IsRequired();
            Property(x => x.IsInUse).HasColumnName(@"IsInUse").HasColumnType("bit").IsRequired();
            Property(x => x.HasMultipleDb).HasColumnName(@"HasMultipleDB").HasColumnType("bit").IsRequired();
        }
    }

}

