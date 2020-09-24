using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public interface ITester
        {
            /// <summary>
            /// Takes two numbers and checks them
            /// </summary>
            /// <param name="a">The value for a</param>
            /// <param name="b">The value for b</param>
            /// <returns>True if a is less than or equal to b, false otherwise</returns>
            bool Check(int a, float b);

            /// <summary>
            /// Takes an array of inputs and counts the number of times each number shows up in the input array
            /// </summary>
            /// <param name="inputs">List of numbers to count instances of</param>
            /// <returns>Map of the input number and how many times it shows up in the array</returns>
            IDictionary<int, int> Count(params int[] inputs);

        }

        public class Tester : ITester
        {
            public bool Check(int a, float b)
            {
                return a <= b;
            }

            public IDictionary<int, int> Count(params int[] inputs)
            {
                return inputs.GroupBy(x => x).ToDictionary(y => y.Key, y => y.Count());
            }
        }

        public class Tester2 : ITester
        {
            public bool Check(int a, float b)
            {
                if (a < b)
                    return true;
                if ((b - a) > 0)
                    return true;
                return false;
            }

            public IDictionary<int, int> Count(params int[] inputs)
            {
                Dictionary<int, int> counts = new Dictionary<int, int>();
                foreach (var i in inputs)
                {
                    if (!counts.ContainsKey(i))
                    {
                        counts.Add(i, 1);
                    }

                    counts[i]++;
                }

                return counts;
            }
        }
    }
}

