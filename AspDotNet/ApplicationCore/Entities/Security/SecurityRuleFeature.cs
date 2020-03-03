using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class SecurityRuleFeature : AuditFields, IBaseEntity
    {
        /// <summary>
        /// We need this for using EfRepository
        /// </summary>
        [NotMapped]
        public int Id { get; set; }

        ///<summary>
        /// FeatureID (Primary key)
        ///</summary>
        public int FeatureId { get; set; }

        ///<summary>
        /// SecurityRuleCode (Primary key)
        ///</summary>
        public int SecurityRuleCode { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Parent SecurityRule pointed by [SecurityRuleFeature].([SecurityRuleCode]) (FK_SecurityRuleFeature_SecurityRule)
        /// </summary>
        public virtual SecurityRule SecurityRule { get; set; }

        public SecurityRuleFeature()
        {
            EntityState = EntityState.Added;
        }
    }
}
