using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day3_2
{
    class Program
    {
        public const int Size = 50000;
        public const int Station = 25000;

        static void Main(string[] args)
        {
            var grid = GetNewGrid();
            var firstWire = File.ReadAllLines("input.txt").First();
            var firstWireInstructions = firstWire.Split(',').Select(x => new Instruction(x)).ToList();

            var secondWire = File.ReadAllLines("input.txt")[1];
            var secondWireInstructions = secondWire.Split(',').Select(x => new Instruction(x)).ToList();

            var firstWirePosition = new Point(Station, Station);
            var firstWireSteps = new Steps();
            foreach (var instruction in firstWireInstructions)
            {
                firstWirePosition = instruction.OperateWithoutCrosses(grid, firstWirePosition, firstWireSteps);
            }

            var stepsList = new List<int>();
            var secondWirePosition = new Point(Station, Station);
            var secondWireSteps = new Steps();
            foreach (var instruction in secondWireInstructions)
            {
                secondWirePosition = instruction.OperateWithCrosses(grid, secondWirePosition, secondWireSteps, stepsList);
            }

            Console.WriteLine($"Least steps: { stepsList.Min() }");
            Console.ReadLine();
        }

        static int[][] GetNewGrid()
        {
            var grid = new int[Size][];
            for (int i = 0; i < Size; i++)
            {
                grid[i] = new int[Size];
            }

            return grid;
        }
    }

    struct Point
    {
        public int X;
        public int Y;

        public Point(int y, int x)
        {
            Y = y;
            X = x;
        }
    }

    class Instruction
    {
        private char direction;
        private int distance;

        public string Action { get; set; }

        public Instruction(string action)
        {
            Action = action;
            Calculate();
        }

        public Point OperateWithoutCrosses(int[][] grid, Point currentPosition, Steps wireSteps)
        {
            return Operate(grid, currentPosition, (y, x) => ++wireSteps.TotalSteps);
        }

        public Point OperateWithCrosses(int[][] grid, Point currentPosition, Steps wireSteps, List<int> steps)
        {
            return Operate(grid, currentPosition, (y, x) =>
            {
                if (grid[y][x] != 0)
                {
                    steps.Add(grid[y][x] + wireSteps.TotalSteps + 1);
                }

                ++wireSteps.TotalSteps;
                return 0;
            });
        }

        private Point Operate(int[][] grid, Point currentPosition, Func<int, int, int> getValue)
        {
            switch (direction)
            {
                case 'L':
                    for (int i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y][currentPosition.X - i] = getValue(currentPosition.Y, currentPosition.X - i);
                    }
                    return new Point(currentPosition.Y, currentPosition.X - distance);
                case 'U':
                    for (int i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y - i][currentPosition.X] = getValue(currentPosition.Y - i, currentPosition.X);
                    }
                    return new Point(currentPosition.Y - distance, currentPosition.X);
                case 'R':
                    for (int i = 1; i <= distance; i++)
                    {
                        grid[currentPosition.Y][currentPosition.X + i] = getValue(currentPosition.Y, currentPosition.X + i);
                    }
                    return new Point(currentPosition.Y, currentPosition.X + distance);
                case 'D':
                    for (int i = 1; i <= distance; i++)
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
            this.distance = int.Parse(new string(Action.Skip(1).ToArray()));
        }
    }

    class Steps
    {
        public int TotalSteps { get; set; } = 0;
    }
}
