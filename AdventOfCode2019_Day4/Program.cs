using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            int sum = 0;
            for (int i = 156218; i <= 652527; i++)
            {
                if (MeetsCriteria(i))
                {
                    sum++;
                }
            }

            Console.WriteLine(sum);            
            Console.ReadLine();
        }

        static bool MeetsCriteria(int number)
        {
            var digits = new[]
            {
                number % 10,
                (number % 100) / 10,
                (number % 1_000) / 100,
                (number % 10_000) / 1_000,
                (number % 100_000) / 10_000,
                number / 100_000
            };

            bool adjacentEquals = false;
            bool decreases = false;

            for (int i = 0; i < 5; i++)
            {
                adjacentEquals |= (digits[i] == digits[i + 1] && digits.Count(x => x == digits[i]) == 2);
                decreases |= digits[i] < digits[i + 1];
            }

            return adjacentEquals && !decreases;
        }
    }
}
