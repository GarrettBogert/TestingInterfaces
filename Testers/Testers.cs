using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testers
{

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
