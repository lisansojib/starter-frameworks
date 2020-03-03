using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// EntityType
    /// </summary>
    public class EntityType : BaseEntity
    {
        ///<summary>
        /// EntityTypeName (length: 100)
        ///</summary>
        public string EntityTypeName { get; set; }

        ///<summary>
        /// IntegerValue
        ///</summary>
        public bool IntegerValue { get; set; }

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

        // Reverse navigation

        /// <summary>
        /// Child EntityTypeValues where [EntityTypeValue].[EntityTypeID] point to this entity (FK_EntityTypeValue_EntityType)
        /// </summary>
        public virtual ICollection<EntityTypeValue> EntityTypeValues { get; set; } // EntityTypeValue.FK_EntityTypeValue_EntityType

        public EntityType()
        {
            IntegerValue = false;
            IsUsed = false;
            EntityTypeValues = new List<EntityTypeValue>();
        }
    }
}
