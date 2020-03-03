using ApplicationCore.Entities;
using ApplicationCore.Exceptions;

namespace Ardalis.GuardClauses
{
    public static class GuardExtensions
    {
        public static void NullEntity(this IGuardClause guardClause, int id, IBaseEntity entity)
        {
            if (entity == null)
            {
                throw new ItemNotFoundException(id);
            }
        }

        public static void NullObject(this IGuardClause guardClause, int id, object obj)
        {
            if (obj == null)
                throw new ItemNotFoundException(id);
        }
    }
}
