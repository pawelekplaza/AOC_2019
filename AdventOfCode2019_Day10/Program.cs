using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdventOfCode2019_Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //var input = File.ReadAllLines("input.txt");

            // [5,8] 33
            //var input = new[] {
            //    "......#.#.",
            //    "#..#.#....",
            //    "..#######.",
            //    ".#.#.###..",
            //    ".#..#.....",
            //    "..#....#.#",
            //    "#..#....#.",
            //    ".##.#..###",
            //    "##...#..#.",
            //    ".#....####"
            //};

            // [1,2] 35
            //var input = new[]
            //{
            //    "#.#...#.#.",
            //    ".###....#.",
            //    ".#....#...",
            //    "##.#.#.#.#",
            //    "....#.#.#.",
            //    ".##..###.#",
            //    "..#...##..",
            //    "..##....##",
            //    "......#...",
            //    ".####.###."
            //};

            // [6,3] 41
            //var input = new[]
            //{
            //    ".#..#..###",
            //    "####.###.#",
            //    "....###.#.",
            //    "..###.##.#",
            //    "##.##.#.#.",
            //    "....###..#",
            //    "..#.#..#.#",
            //    "#..#.#.###",
            //    ".##...##.#",
            //    ".....#.#.."
            //};

            // [11,13] 210
            var input = new[]
            {
                ".#..##.###...#######",
                "##.############..##.",
                ".#.######.########.#",
                ".###.#######.####.#.",
                "#####.##.#.##.###.##",
                "..#####..#.#########",
                "####################",
                "#.####....###.#.#.##",
                "##.#################",
                "#####.##.###..####..",
                "..######..##.#######",
                "####.##.####...##..#",
                ".#####..#.######.###",
                "##...#.##########...",
                "#.##########.#######",
                ".####.#.###.###.#.##",
                "....##.##.###..#####",
                ".#.#.###########.###",
                "#.#.#.#####.####.###",
                "###.##.####.##.#..##"
            };

            //var input = new[]
            //{
            //    "#..#.",
            //    "#....",
            //    "#....",
            //    ".....",
            //    "....."
            //};

            var asteroids = new List<Asteroid>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        asteroids.Add(new Asteroid(new Point(x, y)));
                    }
                }
            }

            var notInLines = new HashSet<NotInLine>();
            foreach (var aster in asteroids)
            {
                var sum = 0;
                var alreadyChecked = new List<Point>();

                foreach (var middleAster in asteroids)
                {
                    if (aster.Coordinates == middleAster.Coordinates)
                    {
                        continue;
                    }

                    var pairsInLine = new List<Point[]>();
                    foreach (var endAster in asteroids)//.Where(x => !notInLines.Contains(new NotInLine(aster.Coordinates, middleAster.Coordinates, x.Coordinates))))
                    {
                        if (aster.Coordinates == endAster.Coordinates ||
                            middleAster.Coordinates == endAster.Coordinates)
                        {
                            continue;
                        }

                        if (alreadyChecked.Contains(middleAster.Coordinates))
                        {
                            continue;
                        }

                        if (IsInTheLine(aster.Coordinates, endAster.Coordinates, middleAster.Coordinates))
                        {
                            alreadyChecked.Add(endAster.Coordinates);
                            pairsInLine.Add(new[] { middleAster.Coordinates, endAster.Coordinates });
                        }
                        //else
                        //{
                        //    notInLines.Add(new NotInLine(aster.Coordinates, middleAster.Coordinates, endAster.Coordinates));
                        //}
                    }

                    if (pairsInLine.Count == 0)
                    {
                        continue;
                    }

                    sum += (pairsInLine.Any(pair => pair.Any(element => element.X > aster.Coordinates.X)) && pairsInLine.Any(pair => pair.Any(element => element.X < aster.Coordinates.X))) ||
                           (pairsInLine.Any(pair => pair.Any(element => element.Y > aster.Coordinates.Y)) && pairsInLine.Any(pair => pair.Any(element => element.Y < aster.Coordinates.Y)))
                        ? pairsInLine.Count - 1 : pairsInLine.Count;
                }

                aster.DetectedAsteroids = asteroids.Count - sum - 1;
            }

            var max = asteroids.Max(x => x.DetectedAsteroids);
            var best = asteroids.FirstOrDefault(x => x.DetectedAsteroids == max);
            Console.WriteLine($"Best station: { best }   Detected asteroids: { best.DetectedAsteroids }");
            Console.WriteLine($"Time: { stopwatch.ElapsedMilliseconds } ms");
            Console.ReadLine();
        }

        static bool IsInTheLine(Point start, Point end, Point point)
        {
            var x = (end.Y - start.Y) / (end.X - start.X);
            var y = (point.Y - start.Y) / (point.X - start.X);

            return x.Equals(y) ||
                   start.X.Equals(point.X) && start.X.Equals(end.X) ||
                   start.Y.Equals(point.Y) && start.Y.Equals(end.Y);
        }
    }

    class Asteroid
    {
        public Point Coordinates { get; set; }
        public int DetectedAsteroids { get; set; }
        public List<Asteroid> HiddenAsteroids { get; set; } = new List<Asteroid>();

        public Asteroid(Point coordinates)
        {
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return $" [{ Coordinates.X }, { Coordinates.Y }] ";
        }
    }

    struct NotInLine
    {
        public Point A { get; }
        public Point B { get; }
        public Point C { get; }

        public NotInLine(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NotInLine nil))
            {
                return base.Equals(obj);
            }

            return (A == nil.A || A == nil.B || A == nil.C) &&
                   (B == nil.A || B == nil.B || B == nil.C) &&
                   (C == nil.A || C == nil.B || C == nil.C);
        }

        public override int GetHashCode()
        {
            return (int)((A.X + B.X + C.X) * 123 + (A.Y + B.Y + C.Y) * 12);
        }
    }
}