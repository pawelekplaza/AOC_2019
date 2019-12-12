using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").First();
            var integers = input.Split(',').Select(int.Parse).ToArray();
            /*
            //var input = "1,1,1,4,99,5,6,0,99";

            const int validResult = 19_690_720;
            var alarm = new Alarm(input);            

            for (int i = 0; i < 100 && alarm[0] != validResult; i++)
            {
                for (int j = 0; j < 100 && alarm[0] != validResult; j++)
                {
                    integers[1] = i;
                    integers[2] = j;
                    alarm.Initialize(integers);
                    alarm.Operate();
                }
            }*/

            integers[1] = 65;
            integers[2] = 33;
            var alarm = new Alarm();
            alarm.Initialize(integers);
            alarm.Operate();

            //Console.WriteLine($"Pair: { alarm[1] } { alarm[2] }");
            Console.WriteLine(alarm.GetResult());
            Console.ReadLine();
        }
    }
}
