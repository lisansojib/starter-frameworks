using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// EntityState
        /// </summary>
        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseEntity()
        {
            EntityState = EntityState.Added;
        }
    }
}
