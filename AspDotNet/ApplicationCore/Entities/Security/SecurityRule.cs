using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class SecurityRule : AuditFields, IBaseEntity
    {
        ///<summary>
        /// SecurityRuleCode (Primary key)
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// SecurityRuleName (length: 250)
        ///</summary>
        public string SecurityRuleName { get; set; }

        ///<summary>
        /// SecurityRuleDescription (length: 500)
        ///</summary>
        public string SecurityRuleDescription { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Child GroupUserSecurityRules where [GroupUserSecurityRule].[SecurityRuleCode] point to this entity (FK_GroupUserSecurityRule_SecurityRule)
        /// </summary>
        public virtual ICollection<GroupUserSecurityRule> GroupUserSecurityRules { get; set; }

        /// <summary>
        /// Child SecurityRuleFaqs where [SecurityRuleFAQ].[SecurityRuleCode] point to this entity (FK_SecurityRuleFAQ_FAQMaster)
        /// </summary>
        public virtual ICollection<SecurityRuleFaq> SecurityRuleFaqs { get; set; }

        /// <summary>
        /// Child SecurityRuleFeatures where [SecurityRuleFeature].[SecurityRuleCode] point to this entity (FK_SecurityRuleFeature_SecurityRule)
        /// </summary>
        public virtual ICollection<SecurityRuleFeature> SecurityRuleFeatures { get; set; } 

        /// <summary>
        /// Child SecurityRuleMenus where [SecurityRuleMenu].[SecurityRuleCode] point to this entity (FK_SecurityRuleMenu_SecurityRule)
        /// </summary>
        public virtual ICollection<SecurityRuleMenu> SecurityRuleMenus { get; set; }

        /// <summary>
        /// Child SecurityRuleReports where [SecurityRuleReport].[SecurityRuleCode] point to this entity (FK_SecurityRuleReport_SecurityRule)
        /// </summary>
        public virtual ICollection<SecurityRuleReport> SecurityRuleReports { get; set; }

        public SecurityRule()
        {
            EntityState = EntityState.Added;
            GroupUserSecurityRules = new List<GroupUserSecurityRule>();
            SecurityRuleFaqs = new List<SecurityRuleFaq>();
            SecurityRuleFeatures = new List<SecurityRuleFeature>();
            SecurityRuleMenus = new List<SecurityRuleMenu>();
            SecurityRuleReports = new List<SecurityRuleReport>();
        }
    }
}
