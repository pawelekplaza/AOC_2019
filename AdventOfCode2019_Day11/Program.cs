using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99";
            var input = File.ReadAllLines("input.txt")[0];

            //var input =
            //"3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99";

            //var input = "101,5,10,10,102,5,10,11,209,-1,99";

            var alarm = new Alarm(input);
            alarm.Operate();            

            //Console.WriteLine(alarm.GetResult());
            alarm.Paint();
            Console.ReadLine();
        }
    }    
}
