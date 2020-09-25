using Testers;
using NUnit.Framework;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;
using System;
using System.Linq;
using System.IO;
using NUnitTestProject1.BaseClasses;

namespace NUnitTestProject1
{
    public struct TesterData<TParameter>
    {
        public ITester tester;
        public TParameter parameters;

        public TesterData(ITester tester, TParameter parameters)
        {
            this.tester = tester;
            this.parameters = parameters;
        }
    }    

    public class TesterDataProvider<TParameter> : TesterBase, IEnumerable<TesterData<TParameter>>
    {
        public IEnumerator<TesterData<TParameter>> GetEnumerator()
        {
            var testers = GetTesters();

            var paramType = typeof(TParameter).Name;
            string directoryOfJson = Path.Combine(Directory.GetCurrentDirectory(), $"DataDrivenTests/TesterTests/{paramType}Tests.json");

            using (StreamReader sr = new StreamReader(directoryOfJson))
            {
                var json = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<TParameter>>(json);
                var cartesian = new List<TesterData<TParameter>>();
                testers.ForEach(t => data.ForEach(d => cartesian.Add(new TesterData<TParameter>(t, d))));
                return cartesian.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterData<TParameter>>)this).GetEnumerator();
    }


    [TestFixture]
    [TestFixtureSource(typeof(TesterDataProvider<TesterCheck>))]
    public class TesterCheckTests
    {
        private readonly TesterData<TesterCheck> input;
        public TesterCheckTests(TesterData<TesterCheck> input)
        {
            this.input = input;
        }

        [Test]
        public void CheckTest()
        {
            CheckTest(input);
        }

        private void CheckTest(TesterData<TesterCheck> input)
        {
            bool result = input.tester.Check(input.parameters.Number1, input.parameters.Number2);
            Assert.AreEqual(input.parameters.Expected, result);
        }
    }

    [TestFixture]
    [TestFixtureSource(typeof(TesterDataProvider<TesterCount>))]
    public class TesterCountTests
    {
        private readonly TesterData<TesterCount> input;
        public TesterCountTests(TesterData<TesterCount> input)
        {
            this.input = input;
        }

        [Test]
        public void CountTest()
        {   
            CountTest(input);
        }

        public void CountTest(TesterData<TesterCount> input)
        {
            var result = input.tester.Count(input.parameters.Numbers);
            Assert.AreEqual(input.parameters.Expected, result);
        }
    }
}