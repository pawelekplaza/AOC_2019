using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day11
{
    public class Alarm : Dictionary<BigInteger, BigInteger>//List<BigInteger>
    {
        private Square currentPosition;
        private BigInteger i = 0;
        private readonly Grid grid = new Grid();
        public BigInteger RelativeBase { get; private set; } = 0;

        public void Initialize(string input)
        {
            Clear();
            var integers = input.Split(',').Select(BigInteger.Parse).ToArray();
            //AddRange(integers);
            for (int x = 0; x < integers.Length; x++)
            {
                Add(x, integers[x]);
            }

            this.currentPosition = grid.GetSquare(0, 0);
            this.currentPosition.Color = Color.White;
        }

        public void Operate()
        {
            var outputs = new List<BigInteger>();
            while (true)
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
                    this[GetModeratePosition(firstParamMode, i + 1)] = this.currentPosition.Color.ToBigInt();
                    i += 2;
                }
                else if (opcode.Equals(4))
                {
                    outputs.Add(GetModerateValue(firstParamMode, i + 1));

                    if (outputs.Count == 2)
                    {
                        MoveRobot(outputs);
                    }

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

        public Alarm()
        {

        }

        public Alarm(string input)
        {
            Initialize(input);
        }

        public void Paint()
        {
            for (int y = 0; y <= this.grid.Max(g => g.Y); y++)
            {
                for (int x = 0; x <= this.grid.Max(g => g.X); x++)
                {
                    if (grid.GetSquare(x, y).Color == Color.White)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
        }

        private void MoveRobot(List<BigInteger> outputs)
        {
            this.currentPosition.Color = outputs[0].ToColor();
            this.currentPosition.PaintingCounter++;

            var positionAfterTurn = this.currentPosition.GetPositionAfterTurn((int)outputs[1]);
            this.currentPosition = grid.GetSquare(positionAfterTurn.X, positionAfterTurn.Y);
            this.currentPosition.HeadingDirection = positionAfterTurn.HeadingDirection;

            outputs.Clear();
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
