using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class GroupUser : AuditFields, IBaseEntity
    {
        ///<summary>
        /// GroupCode (Primary key)
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// GroupName (length: 250)
        ///</summary>
        public string GroupName { get; set; }

        ///<summary>
        /// GroupDescription (length: 500)
        ///</summary>
        public string GroupDescription { get; set; }

        ///<summary>
        /// DefaultApplicationID
        ///</summary>
        public int DefaultApplicationId { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Child GroupUserSecurityRules where [GroupUserSecurityRule].[GroupCode] point to this entity (FK_GroupUserSecurityRule_GroupUser)
        /// </summary>
        public virtual ICollection<GroupUserSecurityRule> GroupUserSecurityRules { get; set; }

        /// <summary>
        /// Child LoginUserAttachedWithGroupUsers where [LoginUserAttachedWithGroupUser].[GroupCode] point to this entity (FK_LoginUserAttachedWithGroupUser_GroupUser)
        /// </summary>
        public virtual ICollection<LoginUserAttachedWithGroupUser> LoginUserAttachedWithGroupUsers { get; set; }

        public GroupUser()
        {
            EntityState = EntityState.Added;
            DefaultApplicationId = 0;
            GroupUserSecurityRules = new List<GroupUserSecurityRule>();
            LoginUserAttachedWithGroupUsers = new List<LoginUserAttachedWithGroupUser>();
        }
    }
}
