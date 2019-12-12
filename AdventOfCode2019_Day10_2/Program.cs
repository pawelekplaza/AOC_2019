using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace AdventOfCode2019_Day10_2
{
    class Program
    {
        public static Point Station = new Point(26, 36);

        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var input = File.ReadAllLines("input.txt");


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

            var pointsToCheck = asteroids
                .Where(x => x.Coordinates != Station)
                .Select(x => GetEndPoint(Station, x.Coordinates, input.Length - 1))
                .Distinct()
                .ToArray();

            var sortedPoints = new List<Point>();

            // |/
            var partOfPoints = pointsToCheck.Where(x => x.X >= Station.X && x.Y.Equals(0)).ToArray();
            Array.Sort(partOfPoints, (x, y) => x.X.CompareTo(y.X));
            sortedPoints.AddRange(partOfPoints);

            // /
            // \
            partOfPoints = pointsToCheck.Where(x => x.X.Equals(input.Length - 1) && !x.Y.Equals(0)).ToArray();
            Array.Sort(partOfPoints, (x, y) => x.Y.CompareTo(y.Y));
            sortedPoints.AddRange(partOfPoints);

            // /\
            partOfPoints = pointsToCheck.Where(x => x.Y.Equals(input.Length - 1) && !x.X.Equals(input.Length - 1)).ToArray();
            Array.Sort(partOfPoints, (x, y) => -x.X.CompareTo(y.X));
            sortedPoints.AddRange(partOfPoints);

            // \
            // /
            partOfPoints = pointsToCheck.Where(x => x.X.Equals(0) && !x.Y.Equals(input.Length - 1)).ToArray();
            Array.Sort(partOfPoints, (x, y) => -x.Y.CompareTo(y.Y));
            sortedPoints.AddRange(partOfPoints);

            // \|
            partOfPoints = pointsToCheck.Where(x => x.Y.Equals(0) && x.X < Station.X).ToArray();
            Array.Sort(partOfPoints, (x, y) => x.X.CompareTo(y.X));
            sortedPoints.AddRange(partOfPoints);

            var counter = 1;
            foreach (var point in sortedPoints)
            {
                var temp = asteroids.Where(x => x.Coordinates != Station && IsInTheLine(Station, point, x.Coordinates));
                var asteroidToBeVaporized = temp.First(x => x.DistanceFromStation == temp.Min(y => y.DistanceFromStation));
                if (counter++ == 200)
                {
                    Console.WriteLine($"200th vaporized asteroid: { asteroidToBeVaporized }");
                }
            }

            Console.WriteLine($"Detected: { pointsToCheck.Length }");
            Console.WriteLine($"Time: { stopwatch.ElapsedMilliseconds } ms");
            Console.ReadLine();
        }

        static bool IsInTheLine(Point start, Point end, Point point)
        {
            var x = (end.Y - start.Y) / (end.X - start.X);
            var y = (point.Y - start.Y) / (point.X - start.X);

            x = Math.Round(x, 5);
            y = Math.Round(y, 5);

            return x.Equals(y) ||
                   start.X.Equals(point.X) && start.X.Equals(end.X) ||
                   start.Y.Equals(point.Y) && start.Y.Equals(end.Y);
        }

        static Point GetEndPoint(Point start, Point asteroidPoint, double max)
        {
            if (asteroidPoint.X.Equals(max) || asteroidPoint.Y.Equals(max) || asteroidPoint.X.Equals(0) || asteroidPoint.Y.Equals(0))
            {
                return asteroidPoint;
            }

            if (start.X.Equals(asteroidPoint.X))
            {
                return new Point(start.X, start.Y > asteroidPoint.Y ? 0 : max);
            }

            if (start.Y.Equals(asteroidPoint.Y))
            {
                return new Point(start.X > asteroidPoint.X ? 0 : max, start.Y);
            }

            var point = new Point(0, GetY(start, asteroidPoint, 0));
            if (start.X > asteroidPoint.X && point.Y >= 0 && point.Y <= max)
            {
                if (!IsInTheLine(start, point, asteroidPoint)) throw new Exception();
                return point;
            }

            point = new Point(max, GetY(start, asteroidPoint, max));
            if (start.X < asteroidPoint.X && point.Y >= 0 && point.Y <= max)
            {
                if (!IsInTheLine(start, point, asteroidPoint)) throw new Exception();
                return point;
            }

            point = new Point(GetX(start, asteroidPoint, 0), 0);
            if (start.Y > asteroidPoint.Y && point.X >= 0 && point.X <= max)
            {
                if (!IsInTheLine(start, point, asteroidPoint)) throw new Exception();
                return point;
            }

            point = new Point(GetX(start, asteroidPoint, max), max);
            if (start.Y < asteroidPoint.Y && point.X >= 0 && point.X <= max)
            {
                if (!IsInTheLine(start, point, asteroidPoint)) throw new Exception();
                return point;
            }

            throw new Exception();
        }

        static double GetY(Point start, Point asteroidPoint, double x)
        {
            return start.Y + (((x - start.X) * (asteroidPoint.Y - start.Y)) / (asteroidPoint.X - start.X));
        }

        static double GetX(Point start, Point asteroidPoint, double y)
        {
            return start.X + (((y - start.Y) * (asteroidPoint.X - start.X)) / (asteroidPoint.Y - start.Y));
        }
    }

    class Asteroid
    {
        public Point Coordinates { get; set; }
        public double DistanceFromStation => Math.Sqrt(Math.Pow(Coordinates.Y - Coordinates.X, 2) + Math.Pow(Program.Station.Y - Program.Station.X, 2));

        public Asteroid(Point coordinates)
        {
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return $" [{ Coordinates.X }, { Coordinates.Y }] ";
        }
    }
}