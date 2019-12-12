using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day7_1
{
    public class PermutationsProvider
    {
        private readonly List<char[]> permutations = new List<char[]>();

        public List<char[]> GetPermutations(char[] list)
        {
            permutations.Clear();
            int x = list.Length - 1;
            GetPer(list, 0, x);
            return permutations;
        }

        private void GetPer(char[] list, int k, int m)
        {
            if (k == m)
            {
                permutations.Add(list);
            }
            else
                for (int i = 0; i <= m; i++)
                {
                    Swap(list, k, i);
                    GetPer(list, k + 1, m);
                    Swap(list, k, i);
                }
        }

        private void Swap(char[] source, int x, int y)
        {
            if (source[x] == source[y])
            {
                return;
            }

            var temp = source[x];
            source[x] = source[y];
            source[y] = temp;
        }
    }
}
