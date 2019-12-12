using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day5
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
            for (int i = 0; i < Count && !shouldHalt;)
            {
                var opcode = this[i] % 100;
                int first = (this[i] % 1_000) / 100;
                int second = (this[i] % 10_000) / 1_000;

                switch (opcode)
                {
                    case 1:
                        var sum = GetModerateValue(first, i + 1) + GetModerateValue(second, i + 2);
                        this[this[i + 3]] = sum;
                        i += 4;
                        break;
                    case 2:
                        var multiplication = GetModerateValue(first, i + 1) * GetModerateValue(second, i + 2);
                        this[this[i + 3]] = multiplication;
                        i += 4;
                        break;
                    case 3:
                        this[this[i + 1]] = int.Parse(Console.ReadLine() ?? "0");
                        i += 2;
                        break;
                    case 4:
                        Console.Write(GetModerateValue(first, i + 1));
                        i += 2;
                        break;
                    case 5:
                        if (GetModerateValue(first, i + 1) != 0)
                        {
                            i = GetModerateValue(second, i + 2);
                        }
                        else
                        {
                            i += 3;
                        }
                        break;
                    case 6:
                        if (GetModerateValue(first, i + 1) == 0)
                        {
                            i = GetModerateValue(second, i + 2);
                        }
                        else
                        {
                            i += 3;
                        }
                        break;
                    case 7:
                        this[this[i + 3]] =
                            GetModerateValue(first, i + 1) < GetModerateValue(second, i + 2)
                            ? 1 : 0;

                        i += 4;
                        break;
                    case 8:
                        this[this[i + 3]] =
                            GetModerateValue(first, i + 1) == GetModerateValue(second, i + 2)
                            ? 1 : 0;

                        i += 4;
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

        private int GetModerateValue(int mode, int offset)
        {
            return mode == 1 ? this[offset] : this[this[offset]];
        }
    }
}
