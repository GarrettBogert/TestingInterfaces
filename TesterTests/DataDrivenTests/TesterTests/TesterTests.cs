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
    //TParameter will be either TesterCount or TesterCheck - representing parameters/arguments coming from a JSON document.
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
            string directoryOfJson = Path.Combine(Directory.GetCurrentDirectory(), $"DataDrivenTests/TesterTests/{paramType}.json");

            using (StreamReader sr = new StreamReader(directoryOfJson))
            {
                var json = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<TParameter>>(json);

                //Cartesian in this case meaning every known ITester being run with every set of Tester inputs given the generic input type TParameter.      
                var cartesian = testers.SelectMany(tester => data.Select(parameters => new 
                    TesterData<TParameter>(tester, parameters)));
                
                return cartesian.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterData<TParameter>>)this).GetEnumerator();
    }


    [TestFixture]
    [TestFixtureSource(typeof(TesterDataProvider<TesterCheckTestInputs>))]
    public class TesterCheckTests
    {
        private readonly TesterData<TesterCheckTestInputs> input;
        public TesterCheckTests(TesterData<TesterCheckTestInputs> input)
        {
            this.input = input;
        }

        [Test]
        public void CheckTest()
        {
            CheckTest(input);
        }

        private void CheckTest(TesterData<TesterCheckTestInputs> input)
        {
            bool result = input.tester.Check(input.parameters.Number1, input.parameters.Number2);
            Assert.AreEqual(input.parameters.Expected, result);
        }
    }

    [TestFixture]
    [TestFixtureSource(typeof(TesterDataProvider<TesterCountTestInputs>))]
    public class TesterCountTests
    {
        private readonly TesterData<TesterCountTestInputs> input;
        public TesterCountTests(TesterData<TesterCountTestInputs> input)
        {
            this.input = input;
        }

        [Test]
        public void CountTest()
        {   
            CountTest(input);
        }

        private void CountTest(TesterData<TesterCountTestInputs> input)
        {
            var result = input.tester.Count(input.parameters.Numbers);
            Assert.AreEqual(input.parameters.Expected, result);
        }
    }
}