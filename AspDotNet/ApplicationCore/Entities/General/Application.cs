using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Application
    /// </summary>
    public class Application : BaseEntity
    {
        ///<summary>
        /// ApplicationName (length: 50)
        ///</summary>
        public string ApplicationName { get; set; }

        ///<summary>
        /// ApplicationDescription (length: 1000)
        ///</summary>
        public string ApplicationDescription { get; set; }

        ///<summary>
        /// IsDefault
        ///</summary>
        public bool IsDefault { get; set; }

        ///<summary>
        /// ApplicationLogo (length: 2147483647)
        ///</summary>
        public byte[] ApplicationLogo { get; set; }

        ///<summary>
        /// ApplicationLogoPath (length: 250)
        ///</summary>
        public string ApplicationLogoPath { get; set; }

        ///<summary>
        /// SequenceNo
        ///</summary>
        public int SequenceNo { get; set; }

        ///<summary>
        /// IsInUse
        ///</summary>
        public bool IsInUse { get; set; }

        ///<summary>
        /// HasMultipleDB
        ///</summary>
        public bool HasMultipleDb { get; set; }

        /// <summary>
        /// Child ApplicationUsers where [ApplicationUser].[ApplicationID] point to this entity (FK_ApplicationUser_Application)
        /// </summary>
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } // ApplicationUser.FK_ApplicationUser_Application
        /// <summary>
        /// Child Menus where [Menu].[ApplicationID] point to this entity (FK_Menu_Application)
        /// </summary>
        public virtual ICollection<Menu> Menus { get; set; } // Menu.FK_Menu_Application

        public Application()
        {
            IsDefault = false;
            SequenceNo = 0;
            IsInUse = false;
            HasMultipleDb = false;
            ApplicationUsers = new List<ApplicationUser>();
            Menus = new List<Menu>();
        }
    }

}
// </auto-generated>
