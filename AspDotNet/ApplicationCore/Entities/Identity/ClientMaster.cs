using System;

namespace ApplicationCore.Entities
{
    public class ClientMaster : BaseEntity
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Active { get; set; }

        public ClientMaster()
        {
            CreatedOn = DateTime.Now;
            Active = true;
        }
    }
}
