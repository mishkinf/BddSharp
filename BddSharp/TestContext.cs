using System;
using System.Collections.Generic;
using System.Linq;

namespace BddSharp
{
    public class TestContext<T> : IGenericDbContext<T> where T : class
    {
        public static List<T> FixturesList = new List<T>();
        public List<T> EntitiesList;

        public TestContext()
        {
            EntitiesList = new List<T>();
            EntitiesList.AddRange(FixturesList);
        }

        // ---------- GenericDbContext Stuff --------------
        public IQueryable<T> Entities
        {
            get
            {
                return Queryable.AsQueryable(EntitiesList);
            }
        }

        public T Insert(T entity, bool saveChanges = true)
        {
            EntitiesList.Add(entity);

            return entity;
        }

        public void Update(T entity, bool saveChanges = true)
        {
        }

        public void Delete(T entity, bool saveChanges = true)
        {
            EntitiesList.Remove(entity);
        }

        public void SaveChanges()
        {
        }

        public void ApplyOriginalValues(T entity)
        {
        }

        // ---------- Testing Framework Stuff --------------

        public void Add(T entity)
        {
            EntitiesList.Add(entity);
        }

        public void AddEntities(List<T> entities)
        {
            EntitiesList.AddRange(entities);
        }

        public TestContext<T> With(List<T> entities)
        {
            this.AddEntities(entities);
            return this;
        }
    }
}
