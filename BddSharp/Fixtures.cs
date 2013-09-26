using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace BddSharp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FixtureLoaderAttribute : Attribute
    {
        public string Context { get; set; }
    }

    public class Fixtures : DynamicDictionary<Object>
    {
        public void Load(DbContext context, Assembly assembly, string ContextName=null)
        {
            // Loads all methods with Attribute "FixtureLoader"
            var members = assembly.GetTypes()
                .SelectMany(x => x.GetMembers())
                .Union(assembly.GetTypes())
                .Where(x => Attribute.IsDefined(x, typeof(FixtureLoaderAttribute)));

            DataFixture.Context = context;
            DataFixture.Fixtures = this;

            foreach (var m in members)
            {
                foreach (var a in m.GetCustomAttributes(true))
                {
                    var attr = a as FixtureLoaderAttribute;
                    if (ContextName == null || attr.Context == ContextName )
                    {
                        m.ReflectedType.GetMethod(m.Name, BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                        context.SaveChanges();
                    }
                }
            }

        }
    }
}
