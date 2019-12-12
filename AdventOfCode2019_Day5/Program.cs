using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").First();
            //var input =
                //"3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";
            //var input = "3,3,1107,-1,8,3,4,3,99";
            //var integers = input.Split(',').Select(int.Parse).ToArray();

            var alarm = new Alarm(input);
            alarm.Operate();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(alarm.GetResult());
            Console.ReadLine();
        }
    }
}
