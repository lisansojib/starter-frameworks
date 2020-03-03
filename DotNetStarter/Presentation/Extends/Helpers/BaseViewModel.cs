using System.Data.Entity;

namespace Presentation.Extends.Helpers
{
    public abstract class BaseBindingModel
    {
        public int Id { get; set; }
        public EntityState EntityState { get;set;}

        public bool IsModified()
        {
            return Id > 0 && EntityState == EntityState.Modified;
        }
    }
}