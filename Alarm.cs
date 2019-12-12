using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class Alarm : List<int>
    {
        public void Initialize(string input)
        {
            Clear();
            var integers = input.Split(',').Select(int.Parse);
            AddRange(integers);
        }

        public void Initialize(int[] input)
        {
            Clear();
            AddRange(input);
        }

        public void Operate()
        {
            bool shouldHalt = false;
            for (int i = 0; i < Count && !shouldHalt; i += 4)
            {
                var opcode = this[i];

                switch (opcode)
                {
                    case 1:
                        var sum = this[this[i + 1]] + this[this[i + 2]];
                        this[this[i + 3]] = sum;
                        break;
                    case 2:
                        var multiplication = this[this[i + 1]] * this[this[i + 2]];
                        this[this[i + 3]] = multiplication;
                        break;
                    case 99:
                        shouldHalt = true;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public string GetResult()
        {
            var strings = this.Select(x => x.ToString());
            return string.Join(",", strings);
        }

        public Alarm()
        {

        }

        public Alarm(string input)
        {
            Initialize(input);
        }
    }
}
