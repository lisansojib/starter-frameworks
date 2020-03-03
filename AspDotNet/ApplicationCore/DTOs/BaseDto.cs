using System.Data.Entity;

namespace ApplicationCore.DTOs
{
    public class BaseDto
    {
        public BaseDto()
        {
            EntityState = EntityState.Modified;
        }

        public int Id { get; set; }
        public EntityState EntityState { get; set; }
    }
}
