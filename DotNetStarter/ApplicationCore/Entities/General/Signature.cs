namespace ApplicationCore.Entities
{
    /// <summary>
    /// Signature
    /// </summary>
    public class Signature : BaseEntity
    {
        ///<summary>
        /// Field (Primary key) (length: 50)
        ///</summary>
        public string Field { get; set; }

        ///<summary>
        /// Dates (Primary key)
        ///</summary>
        public System.DateTime Dates { get; set; }

        ///<summary>
        /// LastNumber
        ///</summary>
        public decimal LastNumber { get; set; }

        ///<summary>
        /// CompanyID (Primary key) (length: 20)
        ///</summary>
        public string CompanyId { get; set; }

        ///<summary>
        /// SiteID (Primary key) (length: 10)
        ///</summary>
        public string SiteId { get; set; }

        public Signature()
        {
            LastNumber = 1m;
        }
    }

}
