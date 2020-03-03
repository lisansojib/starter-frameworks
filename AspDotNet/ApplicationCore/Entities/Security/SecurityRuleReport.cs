using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class SecurityRuleReport : AuditFields, IBaseEntity
    {
        /// <summary>
        /// We need this for using EfRepository
        /// </summary>
        [NotMapped]
        public int Id { get; set; }

        ///<summary>
        /// ReportID (Primary key)
        ///</summary>
        public int ReportId { get; set; }

        ///<summary>
        /// SecurityRuleCode (Primary key)
        ///</summary>
        public int SecurityRuleCode { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Parent ReportSuite pointed by [SecurityRuleReport].([ReportId]) (FK_SecurityRuleReport_ReportSuite)
        /// </summary>
        public virtual ReportSuite ReportSuite { get; set; }

        /// <summary>
        /// Parent SecurityRule pointed by [SecurityRuleReport].([SecurityRuleCode]) (FK_SecurityRuleReport_SecurityRule)
        /// </summary>
        public virtual SecurityRule SecurityRule { get; set; }

        public SecurityRuleReport()
        {
            EntityState = EntityState.Added;
        }
    }
}
