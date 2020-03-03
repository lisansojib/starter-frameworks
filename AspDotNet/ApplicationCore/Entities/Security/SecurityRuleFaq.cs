using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class SecurityRuleFaq : AuditFields, IBaseEntity
    {
        /// <summary>
        /// We need this for using EfRepository
        /// </summary>
        [NotMapped]
        public int Id { get; set; }

        public int FaqMasterId { get; set; }

        ///<summary>
        /// FAQMasterID (Primary key)
        ///</summary>
        public int FAQMasterID { get; set; }

        ///<summary>
        /// SecurityRuleCode (Primary key)
        ///</summary>
        public int SecurityRuleCode { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Parent SecurityRule pointed by [SecurityRuleFAQ].([SecurityRuleCode]) (FK_SecurityRuleFAQ_FAQMaster)
        /// </summary>
        public virtual SecurityRule SecurityRule { get; set; }

        public SecurityRuleFaq()
        {
            EntityState = EntityState.Added;
        }
    }
}
