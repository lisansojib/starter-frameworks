namespace ApplicationCore.Entities
{
    /// <summary>
    /// MenuDependence
    /// </summary>
    public class MenuDependence : BaseEntity
    {
        ///<summary>
        /// MenuID
        ///</summary>
        public int MenuId { get; set; }

        ///<summary>
        /// DependentMenuID
        ///</summary>
        public int DependentMenuId { get; set; }

        ///<summary>
        /// SeqNo
        ///</summary>
        public int SeqNo { get; set; }

        ///<summary>
        /// RefNo (length: 20)
        ///</summary>
        public string RefNo { get; set; }

        ///<summary>
        /// UserWise
        ///</summary>
        public bool UserWise { get; set; }

        ///<summary>
        /// OtherMenuID
        ///</summary>
        public int OtherMenuId { get; set; }

        ///<summary>
        /// OtherDependentMenuID
        ///</summary>
        public int OtherDependentMenuId { get; set; }

        ///<summary>
        /// MenuType
        ///</summary>
        public bool MenuType { get; set; }

        /// <summary>
        /// Parent Menu pointed by [MenuDependence].([DependentMenuId]) (FK_MenuDependence_Menu1)
        /// </summary>
        public virtual Menu DependentMenu { get; set; } // FK_MenuDependence_Menu1

        /// <summary>
        /// Parent Menu pointed by [MenuDependence].([MenuId]) (FK_MenuDependence_Menu)
        /// </summary>
        public virtual Menu Menu_MenuId { get; set; } // FK_MenuDependence_Menu

        public MenuDependence()
        {
            SeqNo = 0;
            RefNo = "";
            UserWise = false;
            OtherMenuId = 0;
            OtherDependentMenuId = 0;
            MenuType = false;
        }
    }
}
