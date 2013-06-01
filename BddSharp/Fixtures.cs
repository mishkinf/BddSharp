using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace BddSharp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FixtureLoaderAttribute : Attribute
    {
    }

    public class Fixtures : DynamicDictionary<Object>
    {
        public void Load(DbContext context)
        {
            // Loads all methods with Attribute "FixtureLoader"
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            var members = callingAssembly.GetTypes()
                .SelectMany(x => x.GetMembers())
                .Union(callingAssembly.GetTypes())
                .Where(x => Attribute.IsDefined(x, typeof(FixtureLoaderAttribute)));

            foreach (var m in members)
            {
                m.ReflectedType.GetMethod(m.Name, BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { this, context });
            }

            context.SaveChanges();
        }

        public void Load(DbContext context, Assembly assembly)
        {
            // Loads all methods with Attribute "FixtureLoader"
            var members = assembly.GetTypes()
                .SelectMany(x => x.GetMembers())
                .Union(assembly.GetTypes())
                .Where(x => Attribute.IsDefined(x, typeof(FixtureLoaderAttribute)));

            foreach (var m in members)
            {
                m.ReflectedType.GetMethod(m.Name, BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { this, context });
            }

            context.SaveChanges();
        }
    }
}
