using System;

namespace ApplicationCore.Entities
{
    public abstract class AuditFields
    {
        public AuditFields()
        {
            AddedBy = 0;
            AddedDate = DateTime.Now;
        }

        public int AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
