using System.Linq;

namespace BddSharp.Web
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        T Insert(T entity, bool saveChanges = true);
        void Update(T entity, bool saveChanges = true);
        void Delete(T entity, bool saveChanges = true);
        void SaveChanges();
        void ApplyOriginalValues(T entity);
    }
}
