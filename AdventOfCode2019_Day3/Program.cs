using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day3
{
    class Program
    {
        public const long Size = 50000;
        public const long Station = 25000;
        public const long FirstWire = 1;

        static void Main(string[] args)
        {
            var grid = GetNewGrid();
            var firstWire = File.ReadAllLines("input.txt").First();
            var firstWireInstructions = firstWire.Split(',').Select(x => new Instruction(x)).ToList();

            var secondWire = File.ReadAllLines("input.txt")[1];
            var secondWireInstructions = secondWire.Split(',').Select(x => new Instruction(x)).ToList();

            var firstWirePosition = new Point(Station, Station);
            foreach (var instruction in firstWireInstructions)
            {
                firstWirePosition = instruction.OperateWithoutCrosses(grid, firstWirePosition);
            }

            var distancesList = new List<long>();
            var secondWirePosition = new Point(Station, Station);
            foreach (var instruction in secondWireInstructions)
            {
                secondWirePosition = instruction.OperateWithCrosses(grid, secondWirePosition, distancesList);
            }

            Console.WriteLine($"Closest distance: { distancesList.Min() }");
            Console.ReadLine();
        }

        static long[][] GetNewGrid()
        {
            var grid = new long[Size][];
            for (long i = 0; i < Size; i++)
            {
                grid[i] = new long[Size];
            }

            return grid;
        }
    }

    struct Point
    {
        public long X;
        public long Y;

        public Point(long y, long x)
        {
            Y = y;
            X = x;
        }
    }

    class Instruction
    {
        private char direction;
        private long distance;

        public string Action { get; set; }

        public Instruction(string action)
        {
            Action = action;
            Calculate();
        }

        public Point OperateWithoutCrosses(long[][] grid, Point currentPosition)
        {
            return Operate(grid, currentPosition, (y, x) => Program.FirstWire);
        }

        public Point OperateWithCrosses(long[][] grid, Point currentPosition, List<long> distances)
        {
            return Operate(grid, currentPosition, (y, x) =>
            {
                if (grid[y][x] == Program.FirstWire)
                {
                    distances.Add(Math.Abs(y - Program.Station) + Math.Abs(x - Program.Station));
                }

                return 3;
            });
        }

        private Point Operate(long[][] grid, Point currentPosition, Func<long, long, long> getValue)
        {
            switch (direction)
            {
                case 'L':
                    for (long i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y][currentPosition.X - i] = getValue(currentPosition.Y, currentPosition.X - i);
                    }
                    return new Point(currentPosition.Y, currentPosition.X - distance);
                case 'U':
                    for (long i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y - i][currentPosition.X] = getValue(currentPosition.Y - i, currentPosition.X);
                    }
                    return new Point(currentPosition.Y - distance, currentPosition.X);
                case 'R':
                    for (long i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y][currentPosition.X + i] = getValue(currentPosition.Y, currentPosition.X + i);
                    }
                    return new Point(currentPosition.Y, currentPosition.X + distance);
                case 'D':
                    for (long i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y + i][currentPosition.X] = getValue(currentPosition.Y + i, currentPosition.X);
                    }
                    return new Point(currentPosition.Y + distance, currentPosition.X);
                default:
                    throw new ArgumentException("Wrong direction.");
            }
        }

        private void Calculate()
        {
            this.direction = Action.First();
            this.distance = long.Parse(new string(Action.Skip(1).ToArray()));
        }
    }
}
