using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //var input = File.ReadAllLines("input.txt");

            // 13s
            var input = new[]
            {
                "<x=-2, y=16, z=11",
                "<x=2, y=-10, z=-7>",
                "<x=20, y=-8, z=8>",
                "<x=-8, y=5, z=-1>"
            };

            //var input = new[]
            //{
            //    "<x=-1, y=0, z=2>",
            //    "<x=2, y=-10, z=-7>",
            //    "<x=4, y=-8, z=8>",
            //    "<x=3, y=5, z=-1>"
            //};

            //var input = new[]
            //{
            //    "<x=6, y=5, z=4>",
            //    "<x=5, y=4, z=5>",
            //    "<x=4, y=6, z=6>"
            //};


            var moons = GetMoons(input);
            var moonPairs = GetMoonPairs(moons);

            var startingMoons = moons.Select(x => x.Copy()).ToList();

            const int step = 1_000_000;
            for (int i = 1, j = 0; ; i++)
            {
                foreach (var moonPair in moonPairs)
                {
                    moonPair.UpdateVelocity();
                }

                foreach (var moon in moons)
                {
                    moon.UpdatePosition();
                }

                if (moons.Select((x, index) => x.Equals(startingMoons[index])).All(x => x))
                {
                    Console.WriteLine($"Steps: { i + j * (BigInteger)step}   Time: { stopwatch.ElapsedMilliseconds }");
                    break;
                }

                if (i % step == 0)
                {
                    j++;
                    //Console.WriteLine($"Processed steps: { i + j++ * (BigInteger)step }   Time: { stopwatch.ElapsedMilliseconds }");
                    i = 0;
                }


                if (moons.Select((x, index) => x.PositionEquals(startingMoons[index])).All(x => x))
                {
                    Console.WriteLine($"Position equals for step: {i}");
                }

                if (moons.Select((x, index) => x.VelocityEquals(startingMoons[index])).All(x => x))
                {
                    Console.WriteLine($"Velocity equals for step: {i}");
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

        static List<MoonPair> GetMoonPairs(List<Moon> moons)
        {
            var moonPairs = new List<MoonPair>();

            for (int i = 0; i < moons.Count; i++)
            {
                for (int j = 0; j < moons.Count; j++)
                {
                    if (moonPairs.Any(pair =>
                        pair.First == moons[i] && pair.Second == moons[j]
                        || pair.First == moons[j] && pair.Second == moons[i])
                        || i == j)
                    {
                        continue;
                    }

                    moonPairs.Add(new MoonPair(moons[i], moons[j]));
                }
            }

            return moonPairs;
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

        public int GetTotalEnergy()
        {
            return Position.GetEnergy() * Velocity.GetEnergy();
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

        public bool PositionEquals(Moon moon)
        {
            //return Position.Equals(moon.Position) && Velocity.Equals(moon.Velocity);
            return Position.Equals(moon.Position);
        }

        public bool VelocityEquals(Moon moon)
        {
            return Velocity.Equals(moon.Velocity);
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

        public int GetEnergy()
        {
            return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        }

        public bool Equals(Point point)
        {
            return X == point.X
                   && Y == point.Y
                   && Z == point.Z;
        }
    }

    public class MoonPair
    {
        public Moon First { get; set; }
        public Moon Second { get; set; }

        public MoonPair(Moon x, Moon y)
        {
            First = x;
            Second = y;
        }

        public void UpdateVelocity()
        {
            if (First.Position.X > Second.Position.X)
            {
                Second.Velocity.X++;
                First.Velocity.X--;
            }
            else if (Second.Position.X > First.Position.X)
            {
                First.Velocity.X++;
                Second.Velocity.X--;
            }

            if (First.Position.Y > Second.Position.Y)
            {
                Second.Velocity.Y++;
                First.Velocity.Y--;
            }
            else if (Second.Position.Y > First.Position.Y)
            {
                First.Velocity.Y++;
                Second.Velocity.Y--;
            }

            if (First.Position.Z > Second.Position.Z)
            {
                Second.Velocity.Z++;
                First.Velocity.Z--;
            }
            else if (Second.Position.Z > First.Position.Z)
            {
                First.Velocity.Z++;
                Second.Velocity.Z--;
            }
        }
    }
}
