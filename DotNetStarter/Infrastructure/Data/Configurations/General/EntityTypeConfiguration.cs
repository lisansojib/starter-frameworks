using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class EntityTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<EntityType>
    {
        public EntityTypeConfiguration()
            : this("dbo")
        {
        }

        public EntityTypeConfiguration(string schema)
        {
            ToTable("EntityType", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"EntityTypeID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.EntityTypeName).HasColumnName(@"EntityTypeName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(100);
            Property(x => x.IntegerValue).HasColumnName(@"IntegerValue").HasColumnType("bit").IsRequired();
            Property(x => x.IsUsed).HasColumnName(@"IsUsed").HasColumnType("bit").IsRequired();
            Property(x => x.AddedBy).HasColumnName(@"AddedBy").HasColumnType("int").IsRequired();
            Property(x => x.DateAdded).HasColumnName(@"DateAdded").HasColumnType("datetime").IsRequired();
            Property(x => x.UpdatedBy).HasColumnName(@"UpdatedBy").HasColumnType("int").IsOptional();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsOptional();
        }
    }

}

