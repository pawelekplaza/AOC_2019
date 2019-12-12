using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day7_1
{
    class Program
    {
        private static List<char[]> permutations = new List<char[]>();        

        static void Main(string[] args)
        {
            var thrusters = new List<int>();
            //var permutationsProvider = new PermutationsProvider();
            //permutations = permutationsProvider.GetPermutations(new[] { '0', '1', '2', '3', '4' });

            permutations = GetPermutations(new[] { '5', '6', '7', '8', '9' }, 5).ToList();

            var input = File.ReadAllLines("input.txt").First();

            //var input = "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";

            foreach (var seed in permutations)
            {
                var alarm1 = new Alarm(input) { Phase = int.Parse(seed[0].ToString()) };
                var alarm2 = new Alarm(input) { Phase = int.Parse(seed[1].ToString()) };
                var alarm3 = new Alarm(input) { Phase = int.Parse(seed[2].ToString()) };
                var alarm4 = new Alarm(input) { Phase = int.Parse(seed[3].ToString()) };
                var alarm5 = new Alarm(input) { Phase = int.Parse(seed[4].ToString()) };
                var lastOutput = 0;
                try
                {                    
                    while (true)
                    {
                        var output = alarm1.Operate(lastOutput);
                        output = alarm2.Operate(output);
                        output = alarm3.Operate(output);
                        output = alarm4.Operate(output);
                        lastOutput = alarm5.Operate(output);
                    }
                }
                catch (Exception)
                {
                    thrusters.Add(lastOutput);
                }                
            }

            Console.WriteLine($"Highest: { thrusters.Max() }");
            Console.ReadLine();
        }

        static IEnumerable<T[]> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t }.ToArray());

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }).ToArray());
        }
    }
}
