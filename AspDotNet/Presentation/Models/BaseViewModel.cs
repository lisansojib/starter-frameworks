using System.Data.Entity;

namespace Presentation.Models
{
    public abstract class BaseViewModel
    {
        public BaseViewModel()
        {
            EntityState = EntityState.Added;
        }

        public int Id { get; set; }
        public EntityState EntityState { get;set;}
        public bool IsModified()
        {
            return Id > 0 && EntityState == EntityState.Modified;
        }
    }
}