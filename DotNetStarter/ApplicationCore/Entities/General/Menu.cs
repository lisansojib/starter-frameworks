using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Menu
    /// </summary>
    public class Menu : BaseEntity
    {
        ///<summary>
        /// ApplicationID
        ///</summary>
        public int ApplicationId { get; set; }

        ///<summary>
        /// ParentID
        ///</summary>
        public int ParentId { get; set; }

        ///<summary>
        /// DockPanel (length: 50)
        ///</summary>
        public string DockPanel { get; set; }

        ///<summary>
        /// MenuCaption (length: 100)
        ///</summary>
        public string MenuCaption { get; set; }

        ///<summary>
        /// PageName (length: 100)
        ///</summary>
        public string PageName { get; set; }

        ///<summary>
        /// TabCaption (length: 100)
        ///</summary>
        public string TabCaption { get; set; }

        ///<summary>
        /// NavigateUrl (length: 250)
        ///</summary>
        public string NavigateUrl { get; set; }

        ///<summary>
        /// ImageUrl (length: 250)
        ///</summary>
        public string ImageUrl { get; set; }

        ///<summary>
        /// TabWidth
        ///</summary>
        public int TabWidth { get; set; }

        ///<summary>
        /// PageWidth
        ///</summary>
        public int PageWidth { get; set; }

        ///<summary>
        /// PageHeight
        ///</summary>
        public int PageHeight { get; set; }

        ///<summary>
        /// SeqNo
        ///</summary>
        public int SeqNo { get; set; }

        ///<summary>
        /// IsVisible
        ///</summary>
        public bool IsVisible { get; set; }

        ///<summary>
        /// RestrictionLimit
        ///</summary>
        public int RestrictionLimit { get; set; }

        ///<summary>
        /// IsAdminOnly
        ///</summary>
        public bool IsAdminOnly { get; set; }

        ///<summary>
        /// SingleUserView
        ///</summary>
        public bool SingleUserView { get; set; }

        ///<summary>
        /// HasAutoNo
        ///</summary>
        public bool HasAutoNo { get; set; }

        ///<summary>
        /// UseCommonInterface
        ///</summary>
        public bool UseCommonInterface { get; set; }

        ///<summary>
        /// ModuleSelection
        ///</summary>
        public int ModuleSelection { get; set; }

        ///<summary>
        /// HasParam
        ///</summary>
        public bool HasParam { get; set; }

        ///<summary>
        /// MLevel
        ///</summary>
        public int MLevel { get; set; }

        // Reverse navigation

        /// <summary>
        /// Child MenuDependences where [MenuDependence].[DependentMenuID] point to this entity (FK_MenuDependence_Menu1)
        /// </summary>
        public virtual ICollection<MenuDependence> MenuDependences_DependentMenuId { get; set; } // MenuDependence.FK_MenuDependence_Menu1
        /// <summary>
        /// Child MenuDependences where [MenuDependence].[MenuID] point to this entity (FK_MenuDependence_Menu)
        /// </summary>
        public virtual ICollection<MenuDependence> MenuDependences_MenuId { get; set; } // MenuDependence.FK_MenuDependence_Menu
        /// <summary>
        /// Child MenuParams where [MenuParam].[MenuID] point to this entity (FK_MenuParam_Menu)
        /// </summary>
        public virtual ICollection<MenuParam> MenuParams { get; set; } // MenuParam.FK_MenuParam_Menu
        /// <summary>
        /// Child MessageQueues where [MessageQueue].[ForMenuID] point to this entity (FK_MessageQueue_Menu1)
        /// </summary>
        public virtual ICollection<MessageQueue> MessageQueues_ForMenuId { get; set; } // MessageQueue.FK_MessageQueue_Menu1
        /// <summary>
        /// Child MessageQueues where [MessageQueue].[MenuID] point to this entity (FK_MessageQueue_Menu)
        /// </summary>
        public virtual ICollection<MessageQueue> MessageQueues_MenuId { get; set; } // MessageQueue.FK_MessageQueue_Menu

        /// <summary>
        /// Child SecurityRuleMenus where [SecurityRuleMenu].[MenuID] point to this entity (FK_SecurityRule_Object_Menu)
        /// </summary>
        public virtual ICollection<SecurityRuleMenu> SecurityRuleMenus { get; set; }

        /// <summary>
        /// Parent Application pointed by [Menu].([ApplicationId]) (FK_Menu_Application)
        /// </summary>
        public virtual Application Application { get; set; } // FK_Menu_Application

        public Menu()
        {
            TabWidth = 0;
            PageWidth = 0;
            PageHeight = 0;
            SeqNo = 0;
            IsVisible = true;
            RestrictionLimit = 0;
            IsAdminOnly = false;
            SingleUserView = false;
            HasAutoNo = false;
            UseCommonInterface = false;
            ModuleSelection = 0;
            HasParam = false;
            MLevel = 0;
            MenuDependences_DependentMenuId = new List<MenuDependence>();
            MenuDependences_MenuId = new List<MenuDependence>();
            MenuParams = new List<MenuParam>();
            MessageQueues_ForMenuId = new List<MessageQueue>();
            MessageQueues_MenuId = new List<MessageQueue>();
            SecurityRuleMenus = new List<SecurityRuleMenu>();
        }
    }
}
