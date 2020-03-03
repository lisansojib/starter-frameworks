using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ApplicationCore.Entities
{
    public class LoginUserTypeHk : IBaseEntity
    {
        ///<summary>
        /// UserTypeID (Primary key)
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// UserType (length: 30)
        ///</summary>
        public string UserType { get; set; }

        public bool HasTeam { get; set; }

        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// Child LoginUsers where [LoginUser].[UserTypeID] point to this entity (FK_LoginUser_LoginUserType_HK)
        /// </summary>
        public virtual ICollection<LoginUser> LoginUsers { get; set; }

        public LoginUserTypeHk()
        {
            EntityState = EntityState.Added;
            HasTeam = false;
            LoginUsers = new List<LoginUser>();
        }
    }
}
