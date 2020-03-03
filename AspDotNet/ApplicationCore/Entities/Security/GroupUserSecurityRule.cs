using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class GroupUserSecurityRule : AuditFields, IBaseEntity
    {
        /// <summary>
        /// We need this for using our EfRepository
        /// </summary>
        [NotMapped]
        public int Id { get; set; }

        ///<summary>
        /// GroupCode (Primary key)
        ///</summary>
        public int GroupCode { get; set; }

        ///<summary>
        /// SecurityRuleCode (Primary key)
        ///</summary>
        public int SecurityRuleCode { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Parent GroupUser pointed by [GroupUserSecurityRule].([GroupCode]) (FK_GroupUserSecurityRule_GroupUser)
        /// </summary>
        public virtual GroupUser GroupUser { get; set; }

        /// <summary>
        /// Parent SecurityRule pointed by [GroupUserSecurityRule].([SecurityRuleCode]) (FK_GroupUserSecurityRule_SecurityRule)
        /// </summary>
        public virtual SecurityRule SecurityRule { get; set; }

        public GroupUserSecurityRule()
        {
            EntityState = EntityState.Added;
        }
    }
}
