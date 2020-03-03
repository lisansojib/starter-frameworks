using ApplicationCore.Entities;

namespace Infrastructure.Data.Configurations
{
    public class EntityTypeValueConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<EntityTypeValue>
    {
        public EntityTypeValueConfiguration()
            : this("dbo")
        {
        }

        public EntityTypeValueConfiguration(string schema)
        {
            ToTable("EntityTypeValue", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"ValueID").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ValueName).HasColumnName(@"ValueName").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            Property(x => x.EntityTypeId).HasColumnName(@"EntityTypeID").HasColumnType("int").IsRequired();
            Property(x => x.IsUsed).HasColumnName(@"IsUsed").HasColumnType("bit").IsRequired();
            Property(x => x.AddedBy).HasColumnName(@"AddedBy").HasColumnType("int").IsRequired();
            Property(x => x.DateAdded).HasColumnName(@"DateAdded").HasColumnType("datetime").IsRequired();
            Property(x => x.UpdatedBy).HasColumnName(@"UpdatedBy").HasColumnType("int").IsOptional();
            Property(x => x.DateUpdated).HasColumnName(@"DateUpdated").HasColumnType("datetime").IsOptional();

            // Foreign keys
            HasRequired(a => a.EntityType).WithMany(b => b.EntityTypeValues).HasForeignKey(c => c.EntityTypeId).WillCascadeOnDelete(false); // FK_EntityTypeValue_EntityType
        }
    }

}

