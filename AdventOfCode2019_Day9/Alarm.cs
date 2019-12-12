using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day9
{
    public class Alarm : Dictionary<BigInteger, BigInteger>//List<BigInteger>
    {
        public BigInteger RelativeBase { get; private set; } = 0;

        public void Initialize(string input)
        {
            Clear();
            var integers = input.Split(',').Select(BigInteger.Parse).ToArray();
            //AddRange(integers);
            for (int i = 0; i < integers.Length; i++)
            {
                Add(i, integers[i]);
            }
        }

        public void Operate()
        {
            bool shouldHalt = false;
            for (BigInteger i = 0; ; )
            {
                if (i < 0)
                {
                    throw new ArgumentException("Negative address is invalid.");
                }

                var opcode = this[i] % 100;
                var firstParamMode = (this[i] % 1_000) / 100;
                var secondParamMode = (this[i] % 10_000) / 1_000;
                var thirdParamMode = (this[i] % 100_000) / 10_000;

                if (opcode.Equals(1))
                {
                    var sum = GetModerateValue(firstParamMode, i + 1) + GetModerateValue(secondParamMode, i + 2);
                    this[GetModeratePosition(thirdParamMode, i + 3)] = sum;
                    i += 4;
                }
                else if (opcode.Equals(2))
                {
                    var multiplication = GetModerateValue(firstParamMode, i + 1) * GetModerateValue(secondParamMode, i + 2);
                    this[GetModeratePosition(thirdParamMode, i + 3)] = multiplication;
                    i += 4;
                }
                else if (opcode.Equals(3))
                {
                    this[GetModeratePosition(firstParamMode, i + 1)] = BigInteger.Parse(Console.ReadLine() ?? "0");
                    i += 2;
                }
                else if (opcode.Equals(4))
                {
                    var output = GetModerateValue(firstParamMode, i + 1);
                    Console.Write(output);
                    i += 2;
                }
                else if (opcode.Equals(5))
                {
                    if (GetModerateValue(firstParamMode, i + 1) != 0)
                    {
                        i = GetModerateValue(secondParamMode, i + 2);
                    }
                    else
                    {
                        i += 3;
                    }
                }
                else if (opcode.Equals(6))
                {
                    if (GetModerateValue(firstParamMode, i + 1) == 0)
                    {
                        i = GetModerateValue(secondParamMode, i + 2);
                    }
                    else
                    {
                        i += 3;
                    }
                }
                else if (opcode.Equals(7))
                {
                    this[GetModeratePosition(thirdParamMode, i + 3)] =
                        GetModerateValue(firstParamMode, i + 1) < GetModerateValue(secondParamMode, i + 2)
                            ? 1 : 0;
                    i += 4;
                }
                else if (opcode.Equals(8))
                {
                    this[GetModeratePosition(thirdParamMode, i + 3)] =
                        GetModerateValue(firstParamMode, i + 1) == GetModerateValue(secondParamMode, i + 2)
                            ? 1 : 0;
                    i += 4;
                }
                else if (opcode.Equals(9))
                {
                    RelativeBase += GetModerateValue(firstParamMode, i + 1);
                    i += 2;
                }
                else if (opcode.Equals(99))
                {
                    break;
                }
                else
                {
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

        private BigInteger GetModerateValue(BigInteger mode, BigInteger offset)
        {
            if (mode.Equals(0))
            {
                if (!ContainsKey(this[offset]))
                {
                    Add(this[offset], 0);
                }

                return this[this[offset]];
            }

            if (mode.Equals(1))
            {
                if (!ContainsKey(offset))
                {
                    Add(offset, 0);
                }

                return this[offset];
            }

            if (mode.Equals(2))
            {
                if (!ContainsKey(this[offset] + RelativeBase))
                {
                    Add(this[offset] + RelativeBase, 0);
                }

                return this[this[offset] + RelativeBase];
            }

            throw new ArgumentException("Invalid mode.");
        }

        private BigInteger GetModeratePosition(BigInteger mode, BigInteger offset)
        {
            if (mode.Equals(0))
            {
                return this[offset];
            }

            if (mode.Equals(2))
            {
                return this[offset] + RelativeBase;
            }

            throw new ArgumentException("Invalid position mode.");
        }
    }
}
