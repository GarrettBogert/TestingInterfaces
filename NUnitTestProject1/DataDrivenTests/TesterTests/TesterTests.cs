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
    public struct TesterCheckData
    {
        public ITester tester;
        public TesterCheckParameters parameters;
        public TesterCheckData(ITester x, TesterCheckParameters y)
        {
            this.tester = x;
            this.parameters = y;
        }
    }

    public struct TesterCountData
    {
        public ITester tester;
        public TesterCountParameters parameters;
        public TesterCountData(ITester x, TesterCountParameters y)
        {
            this.tester = x;
            this.parameters = y;
        }
    }

    public class TesterCheckProvider : TesterBase, IEnumerable<TesterCheckData>
    {
        public IEnumerator<TesterCheckData> GetEnumerator()
        {
            var testers = GetTesters();
            //TODO: Make this hardcoded path simpler and more programmatic. It works for now though.
            string directoryOfJson = Path.Combine(Directory.GetCurrentDirectory(), "DataDrivenTests/TesterTests/TesterCheckTests.json");

            using (StreamReader sr = new StreamReader(directoryOfJson))
            {
                var json = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<TesterCheckParameters>>(json);
                var cartesian = new List<TesterCheckData>();
                testers.ForEach(t => data.ForEach(d => cartesian.Add(new TesterCheckData(t, d))));
                return cartesian.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterCheckData>)this).GetEnumerator();
    }
    //TODO: Make a single generic provider that can provide count and check data, embarassingly programmatic here.
    public class TesterCountProvider : TesterBase, IEnumerable<TesterCountData>
    {
        public IEnumerator<TesterCountData> GetEnumerator()
        {
            var testers = GetTesters();
            //TODO: Make this hardcoded path simpler and more programmatic. It works for now though.
            string directoryOfJson = Path.Combine(Directory.GetCurrentDirectory(), "DataDrivenTests/TesterTests/TesterCountTests.json");

            using (StreamReader sr = new StreamReader(directoryOfJson))
            {
                var json = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<TesterCountParameters>>(json);
                var cartesian = new List<TesterCountData>();
                testers.ForEach(t => data.ForEach(d => cartesian.Add(new TesterCountData(t, d))));
                return cartesian.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterCountData>)this).GetEnumerator();
    }


    [TestFixture]
    [TestFixtureSource(typeof(TesterCheckProvider))]
    public class TesterCheckTests
    {
        private readonly TesterCheckData input;
        public TesterCheckTests(TesterCheckData input)
        {
            this.input = input;
        }

        [Test]
        public void CheckTest()
        {
            CheckTest(input);
        }

        private void CheckTest(TesterCheckData input)
        {
            bool result = input.tester.Check(input.parameters.Number1, input.parameters.Number2);
            Assert.AreEqual(input.parameters.Expected, result);
        }
    }

    [TestFixture]
    [TestFixtureSource(typeof(TesterCountProvider))]
    public class TesterCountTests
    {
        private readonly TesterCountData input;
        public TesterCountTests(TesterCountData input)
        {
            this.input = input;
        }

        [Test]
        public void CountTest()
        {
            //hard coding values for now 
            CountTest(input);
        }

        public void CountTest(TesterCountData input)
        {
            var result = input.tester.Count(input.parameters.Numbers);
            Assert.AreEqual(input.parameters.Expected, result);
        }
    }
}