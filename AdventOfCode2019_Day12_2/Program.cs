using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day12_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //var input = File.ReadAllLines("input.txt");

            // 13s
            //var input = new[]
            //{
            //    "<x=-2, y=16, z=11",
            //    "<x=2, y=-10, z=-7>",
            //    "<x=20, y=-8, z=8>",
            //    "<x=-8, y=5, z=-1>"
            //};

            var input = new[]
            {
                "<x=-1, y=0, z=2>",
                "<x=2, y=-10, z=-7>",
                "<x=4, y=-8, z=8>",
                "<x=3, y=5, z=-1>"
            };


            var moons = GetMoons(input);

            var startingMoons = moons.Select(x => x.Copy()).ToList();

            const int step = 1_000_000;
            for (int i = 1, j = 0; ; i++)
            {
                Move(moons);

                if (moons.Select((x, index) => x.Equals(startingMoons[index])).All(x => x))
                {
                    Console.WriteLine($"Steps: { i + j * (BigInteger)step}   Time: { stopwatch.ElapsedMilliseconds }");
                    break;
                }

                if (i % step == 0)
                {
                    Console.WriteLine($"Processed steps: { i + j++ * (BigInteger)step }   Time: { stopwatch.ElapsedMilliseconds }");
                    i = 0;
                }
            }

            //Console.WriteLine($"{moons.Sum(x => x.GetTotalEnergy())}");
            Console.ReadLine();
        }

        static List<Moon> GetMoons(string[] input)
        {
            var moons = new List<Moon>();

            foreach (var line in input)
            {
                var moon = new Moon(new Point());

                var skipped = line.Split(' ');
                moon.Position.X = int.Parse(skipped[0].Replace("<x=", "").Replace(",", ""));
                moon.Position.Y = int.Parse(skipped[1].Replace("y=", "").Replace(",", ""));
                moon.Position.Z = int.Parse(skipped[2].Replace("z=", "").Replace(">", ""));

                moons.Add(moon);
            }

            return moons;
        }

        static void Move(List<Moon> moons)
        {
            foreach (var moon in moons)
            {
                var except = moons.Except(new[] { moon });

                int sumX = 0, sumY = 0, sumZ = 0;
                foreach (var ex in except)
                {
                    sumX += ex.Position.X;
                    sumY += ex.Position.Y;
                    sumZ += ex.Position.Z;
                }

                moon.Velocity.X -= sumX;
                moon.Velocity.Y -= sumY;
                moon.Velocity.Z -= sumZ;
            }

            foreach (var moon in moons)
            {
                moon.UpdatePosition();
            }
        }
    }

    public class Moon
    {
        public Point Position { get; set; }
        public Point Velocity { get; set; }

        public Moon()
        {
            Velocity = new Point(0, 0, 0);
        }

        public Moon(Point position) : this()
        {
            Position = position;
        }

        public void UpdatePosition()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            Position.Z += Velocity.Z;
        }

        public Moon Copy()
        {
            var copy = new Moon(new Point(Position.X, Position.Y, Position.Z));
            copy.Velocity = new Point(Velocity.X, Velocity.Y, Velocity.Z);
            return copy;
        }

        public bool Equals(Moon moon)
        {
            return Position.Equals(moon.Position) && Velocity.Equals(moon.Velocity);
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point()
        {

        }

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(Point point)
        {
            return X == point.X
                   && Y == point.Y
                   && Z == point.Z;
        }
    }
}
