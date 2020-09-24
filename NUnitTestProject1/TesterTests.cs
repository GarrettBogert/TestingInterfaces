using Testers;
using NUnit.Framework;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections;

namespace NUnitTestProject1
{

    public class TesterCheckParametersProvider : IEnumerable<TesterCheckParameters>
    {
        public IEnumerator<TesterCheckParameters> GetEnumerator()
        {
            //Hardcoding just to prove out the test fixture source part of this solution.
            //TODO: actually read and deserialize the Check parameters from the json file.
            var data = new List<TesterCheckParameters> { new TesterCheckParameters { Number1 = 6, Number2 = 7, Expected = true }, new TesterCheckParameters { Number1 = 7, Number2 = 7, Expected = true } };
            //TODO: Also find a way to represent the tester class in the JSON file. Perhaps this method can use Reflection to find the Type to instantiate and bind to the TesterCheckParameters?

            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TesterCheckParameters>)this).GetEnumerator();
    }


    [TestFixture]
    [TestFixtureSource(typeof(TesterCheckParametersProvider))]
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