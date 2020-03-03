using System;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// EntityTypeValue
    /// </summary>
    public class EntityTypeValue : BaseEntity
    {
        ///<summary>
        /// ValueName (length: 50)
        ///</summary>
        public string ValueName { get; set; }

        ///<summary>
        /// EntityTypeID
        ///</summary>
        public int EntityTypeId { get; set; }

        ///<summary>
        /// IsUsed
        ///</summary>
        public bool IsUsed { get; set; }

        ///<summary>
        /// AddedBy
        ///</summary>
        public int AddedBy { get; set; }

        ///<summary>
        /// DateAdded
        ///</summary>
        public DateTime DateAdded { get; set; }

        ///<summary>
        /// UpdatedBy
        ///</summary>
        public int? UpdatedBy { get; set; }

        ///<summary>
        /// DateUpdated
        ///</summary>
        public DateTime? DateUpdated { get; set; }

        // Foreign keys

        /// <summary>
        /// Parent EntityType pointed by [EntityTypeValue].([EntityTypeId]) (FK_EntityTypeValue_EntityType)
        /// </summary>
        public virtual EntityType EntityType { get; set; } // FK_EntityTypeValue_EntityType

        public EntityTypeValue()
        {
            IsUsed = false;
        }
    }
}
