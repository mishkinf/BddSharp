using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BddSharp
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public static DbContext DataContext { get; set; }

        public IQueryable<T> Entities
        {
            get
            {
                return Queryable.AsQueryable(DataContext.Set<T>());
            }
        }

        public GenericRepository(DbContext context)
        {
            string connString = "Data Source=.;Initial Catalog=RestaurantExample;Integrated Security=True";
//            string connString = ConfigurationManager.ConnectionStrings[contextName].ConnectionString;
            DataContext = context;
        }

        public T Insert(T entity, bool saveChanges = true)
        {
            var entry = DataContext.Entry(entity);
            entry.State = EntityState.Added;

            if (saveChanges)
                DataContext.SaveChanges();

            return entry.Entity;
        }

        public void Update(T entity, bool saveChanges = true)
        {
            DataContext.Entry(entity).State = EntityState.Modified;
            if (saveChanges)
            {
                DataContext.SaveChanges();
            }
        }

        public void Delete(T entity, bool saveChanges = true)
        {
            DataContext.Entry(entity).State = EntityState.Deleted;
            if (saveChanges)
            {
                DataContext.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }

        public void ApplyOriginalValues(T entity)
        {
            var context = ((IObjectContextAdapter)DataContext).ObjectContext;
            var entry = context.ObjectStateManager.GetObjectStateEntry(entity);
            entry.ApplyOriginalValues(entity);
        }

    }
}
