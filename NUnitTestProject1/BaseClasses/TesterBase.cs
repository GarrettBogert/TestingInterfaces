using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testers;

namespace NUnitTestProject1.BaseClasses
{
    public class TesterBase
    {
        public List<ITester> GetTesters()
        {
            var type = typeof(ITester);
            var testers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface)//Without !p.IsInterface, I'll get the interface with the concrete types.
                .ToList()
                .Select(t => Activator.CreateInstance(Type.GetType(t.AssemblyQualifiedName)) as ITester)
                .ToList();
            return testers;
        }
    }
}
