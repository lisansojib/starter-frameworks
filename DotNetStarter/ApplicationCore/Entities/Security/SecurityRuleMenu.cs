using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class SecurityRuleMenu : AuditFields, IBaseEntity
    {
        /// <summary>
        /// We need this for using EfRepository
        /// </summary>
        [NotMapped]
        public int Id { get; set; }

        ///<summary>
        /// MenuID (Primary key)
        ///</summary>
        public int MenuId { get; set; }

        ///<summary>
        /// SecurityRuleCode (Primary key)
        ///</summary>
        public int SecurityRuleCode { get; set; }

        public bool? CanSelect { get; set; }

        public bool? CanInsert { get; set; }

        public bool? CanUpdate { get; set; }

        public bool? CanDelete { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Parent Menu pointed by [SecurityRuleMenu].([MenuId]) (FK_SecurityRule_Object_Menu)
        /// </summary>
        public virtual Menu Menu { get; set; }

        /// <summary>
        /// Parent SecurityRule pointed by [SecurityRuleMenu].([SecurityRuleCode]) (FK_SecurityRuleMenu_SecurityRule)
        /// </summary>
        public virtual SecurityRule SecurityRule { get; set; }

        public SecurityRuleMenu()
        {
            EntityState = EntityState.Added;
            CanSelect = false;
            CanInsert = false;
            CanUpdate = false;
            CanDelete = false;
        }
    }
}
