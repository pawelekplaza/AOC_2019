using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day13
{
    public class Alarm : Dictionary<BigInteger, BigInteger>//List<BigInteger>
    {
        private Point lastBallPosition;
        private BigInteger i = 0;
        private BigInteger currentScore;
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
                    PrintGame();
                    MovePaddle(firstParamMode);                    

                    //this[GetModeratePosition(firstParamMode, i + 1)] = BigInteger.Parse(Console.ReadLine());

                    //await Task.Delay(1000);

                    i += 2;
                }
                else if (opcode.Equals(4))
                {
                    outputs.Add(GetModerateValue(firstParamMode, i + 1));

                    if (outputs.Count == 3)
                    {
                        if (outputs[0] == -1 && outputs[1] == 0)
                        {
                            this.currentScore  = outputs[2];
                            PrintGame();
                            outputs.Clear();
                        }
                        else
                        {
                            SaveLastBall();
                            DrawTile(outputs);
                        }
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

        private void SaveLastBall()
        {
            var lastBall = this.grid.GetBall();
            if (lastBall == null)
            {
                return;
            }

            this.lastBallPosition = new Point(lastBall.X, lastBall.Y);
        }

        private void DrawTile(List<BigInteger> outputs)
        {
            var tile = grid.GetTile((int) outputs[0], (int) outputs[1]);
            tile.Type = outputs[2].ToObjectType();

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

        private void PrintGame()
        {            
            var stringBuilder = new StringBuilder();
            for (int y = 0; y <= 25; y++)
            {
                for (int x = 0; x < 80; x++)
                {
                    var tile = grid.GetTile(x, y);

                    switch (tile.Type)
                    {
                        case ObjectType.Wall:
                            stringBuilder.Append('|');
                            break;
                        case ObjectType.Block:
                            stringBuilder.Append('X');
                            break;
                        case ObjectType.Paddle:
                            stringBuilder.Append('T');
                            break;
                        case ObjectType.Ball:
                            stringBuilder.Append('O');
                            break;
                        case ObjectType.Empty:
                            stringBuilder.Append(' ');
                            break;
                    }
                }

                stringBuilder.AppendLine();
            }
            
            stringBuilder.AppendLine($"Score: { this.currentScore }");
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder.ToString());
        }

        private void MovePaddle(BigInteger firstParamMode)
        {
            var ball = this.grid.GetBall();
            var currentBallPosition = new Point(ball.X, ball.Y);

            var paddle = this.grid.GetPaddle();
            var paddlePosition = new Point(paddle.X, paddle.Y);

            if (currentBallPosition.X == lastBallPosition.X)
            {
                this[GetModeratePosition(firstParamMode, i + 1)] = 1;
                return;
            }            

            int newPaddleXPosition;
            Move ballDirection = currentBallPosition.X < lastBallPosition.X
                ? Move.Left
                : Move.Right;

            Point nextBallPosition;

            if (currentBallPosition.Y > lastBallPosition.Y)
            {
                nextBallPosition = ballDirection == Move.Left
                    ? new Point(currentBallPosition.X - 1, currentBallPosition.Y + 1)
                    : new Point(currentBallPosition.X + 1, currentBallPosition.Y + 1);
            }
            else
            {
                nextBallPosition = ballDirection == Move.Left
                    ? new Point(currentBallPosition.X - 1, currentBallPosition.Y - 1)
                    : new Point(currentBallPosition.X + 1, currentBallPosition.Y - 1);
            }

            var nextTile = this.grid.GetTile(nextBallPosition.X, nextBallPosition.Y);

            if (nextTile.WillBounce())
            {
                newPaddleXPosition = currentBallPosition.X + (ballDirection == Move.Left ? -1 : 1);
            }
            else if (currentBallPosition.Y == 23 && paddlePosition.X == currentBallPosition.X)
            {
                newPaddleXPosition = currentBallPosition.X;
            }
            else
            {
                newPaddleXPosition = ballDirection == Move.Left
                    ? currentBallPosition.X - 1
                    : currentBallPosition.X + 1;
            }

            if (newPaddleXPosition > paddlePosition.X)
            {
                this[GetModeratePosition(firstParamMode, i + 1)] = 1;
            }
            else if (newPaddleXPosition < paddlePosition.X)
            {
                this[GetModeratePosition(firstParamMode, i + 1)] = -1;
            }
            else if (newPaddleXPosition == paddlePosition.X)
            {
                this[GetModeratePosition(firstParamMode, i + 1)] = 0;
            }
            else
            {
                throw new Exception("Wrong conditions.");
            }
        }
    }
}
