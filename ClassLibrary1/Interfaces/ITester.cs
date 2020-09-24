using System.Collections.Generic;

namespace Testers
{
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
}
