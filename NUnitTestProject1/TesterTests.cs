using Testers;
using NUnit.Framework;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using System;
using System.Linq;

namespace NUnitTestProject1
{
    public struct TesterCheck
    {
        public ITester tester;
        public TesterCheckParameters parameters;
        public TesterCheck(ITester x, TesterCheckParameters y)
        {
            this.tester = x;
            this.parameters = y;
        }
    }
    public class TesterCheckProvider : IEnumerable<TesterCheck>
    {
        public IEnumerator<TesterCheck> GetEnumerator()
        {           
            var data = new List<TesterCheckParameters> { new TesterCheckParameters { Number1 = 6, Number2 = 7, Expected = true }, new TesterCheckParameters { Number1 = 7, Number2 = 7, Expected = true } };          
            var type = typeof(ITester);
            var testers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface)//Without !isInterface, the interface itself gets returned, I only want concrete.
                .ToList()
                .Select(t => Activator.CreateInstance(Type.GetType(t.AssemblyQualifiedName)) as ITester)
                .ToList();

            //I really wanted these next two lines to be a single Linq statement. I know its possible...
            var cartesian = new List<TesterCheck>();
            testers.ForEach(t => data.ForEach(d => cartesian.Add(new TesterCheck(t, d))));
            return cartesian.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterCheck>)this).GetEnumerator();
    }


    [TestFixture]
    [TestFixtureSource(typeof(TesterCheckProvider))]
    public class TesterTests
    {
        private readonly TesterCheckParameters parameters;
        public TesterTests(TesterCheckParameters parameters)
        {
            this.parameters = parameters;
        }

        [Test]
        public void CheckTester()
        {
            var tester = new Tester();
            CheckTest(tester, parameters);
        }

        [Test]
        public void CheckTester2()
        {
            var tester = new Tester2();
            CheckTest(tester, parameters);
        }

        public void CheckTest(ITester tester, TesterCheckParameters parameters)
        {
            bool result = tester.Check(parameters.Number1, parameters.Number2);
            Assert.AreEqual(parameters.Expected, result);
        }


        [SetUp]
        public void Setup()
        {
        }
    }
}