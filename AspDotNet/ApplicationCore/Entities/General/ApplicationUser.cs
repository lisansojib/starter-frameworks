namespace ApplicationCore.Entities
{
    /// <summary>
    /// ApplicationUser
    /// </summary>
    public class ApplicationUser : BaseEntity
    {
        ///<summary>
        /// ApplicationID (Primary key)
        ///</summary>
        public int ApplicationId { get; set; }

        ///<summary>
        /// UserCode (Primary key)
        ///</summary>
        public int UserCode { get; set; }

        ///<summary>
        /// IsDefault
        ///</summary>
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Parent Application pointed by [ApplicationUser].([ApplicationId]) (FK_ApplicationUser_Application)
        /// </summary>
        public virtual Application Application { get; set; } // FK_ApplicationUser_Application

        public ApplicationUser()
        {
            IsDefault = false;
        }
    }
}
