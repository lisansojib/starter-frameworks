namespace ApplicationCore.Entities
{
    /// <summary>
    /// MenuParam
    /// </summary>
    public class MenuParam : BaseEntity
    {
        ///<summary>
        /// ParamType (length: 50)
        ///</summary>
        public string ParamType { get; set; }

        ///<summary>
        /// ParamValue (length: 50)
        ///</summary>
        public string ParamValue { get; set; }

        ///<summary>
        /// SeqNo (Primary key)
        ///</summary>
        public int SeqNo { get; set; }

        // Foreign keys

        /// <summary>
        /// Parent Menu pointed by [MenuParam].([MenuId]) (FK_MenuParam_Menu)
        /// </summary>
        public virtual Menu Menu { get; set; } // FK_MenuParam_Menu
    }
}
