using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class LoginUserAttachedWithGroupUser : AuditFields, IBaseEntity
    {
        /// <summary>
        /// We need this for using EfRepository
        /// </summary>
        [NotMapped]
        public int Id { get; set; }

        ///<summary>
        /// UserCode (Primary key)
        ///</summary>
        public int UserCode { get; set; }

        ///<summary>
        /// GroupCode (Primary key)
        ///</summary>
        public int GroupCode { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }
        
        /// <summary>
        /// Parent GroupUser pointed by [LoginUserAttachedWithGroupUser].([GroupCode]) (FK_LoginUserAttachedWithGroupUser_GroupUser)
        /// </summary>
        public virtual GroupUser GroupUser { get; set; } 

        /// <summary>
        /// Parent LoginUser pointed by [LoginUserAttachedWithGroupUser].([UserCode]) (FK_LoginUserAttachedWithGroupUser_LoginUser)
        /// </summary>
        public virtual LoginUser LoginUser { get; set; }

        public LoginUserAttachedWithGroupUser()
        {
            EntityState = EntityState.Added;
        }
    }
}
