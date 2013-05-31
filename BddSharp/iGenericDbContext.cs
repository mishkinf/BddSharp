using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BddSharp
{
    public interface IGenericDbContext<T> where T : class
    {
        IQueryable<T> Entities { get; }
        T Insert(T entity, bool saveChanges = true);
        void Update(T entity, bool saveChanges = true);
        void Delete(T entity, bool saveChanges = true);
        void SaveChanges();
        void ApplyOriginalValues(T entity);
    }
}
