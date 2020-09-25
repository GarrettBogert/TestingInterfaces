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

    public struct TesterCount
    {
        public ITester tester;
        public TesterCountParameters parameters;
        public TesterCount(ITester x, TesterCountParameters y)
        {
            this.tester = x;
            this.parameters = y;
        }
    }

    public class TesterCountParameters
    {
        public int[] numbers { get; set; }
    }

    public class TesterCheckProvider : TesterBase, IEnumerable<TesterCheck>
    {
        public IEnumerator<TesterCheck> GetEnumerator()
        {  
            var testers = GetTesters();
            //TODO: Make this hardcoded path simpler and more programmatic. It works for now though.
            string directoryOfJson = Path.Combine(Directory.GetCurrentDirectory(), "DataDrivenTests/TesterTests/TesterTests.json");

            using (StreamReader sr = new StreamReader(directoryOfJson))
            {
                var json = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<TesterCheckParameters>>(json);
                var cartesian = new List<TesterCheck>();
                testers.ForEach(t => data.ForEach(d => cartesian.Add(new TesterCheck(t, d))));
                return cartesian.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterCheck>)this).GetEnumerator();
    }


    [TestFixture]
    [TestFixtureSource(typeof(TesterCheckProvider))]
    public class TesterTests
    {
        private readonly TesterCheck input;
        public TesterTests(TesterCheck input)
        {
            this.input = input;
        }

        [Test]
        public void CheckTest()
        {           
            CheckTest(input.tester, input);
        }

        [Test]
        public void CountTest()
        {
            //hard coding values for now 
            CountTest(input.tester, new int[] { 2, 2, 2, 3, 4, 1 });
        }

        public void CheckTest(ITester tester, TesterCheck input)
        {
            bool result = tester.Check(input.parameters.Number1, input.parameters.Number2);
            Assert.AreEqual(input.parameters.Expected, result);
        }

        public void CountTest(ITester tester, int[] numbers)
        {
            var result = tester.Count(numbers);
            Assert.AreEqual(input.parameters.Expected, result);
        }


        [SetUp]
        public void Setup()
        {
        }
    }
}