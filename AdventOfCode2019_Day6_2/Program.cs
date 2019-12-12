using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day6_2
{
    class Program
    {
        private static Dictionary<string, SpaceObject> cache = new Dictionary<string, SpaceObject>();
        private static List<PathPair> pathPairs = new List<PathPair>();
        private static List<ExceptionPair> exceptionPairs = new List<ExceptionPair>();

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            /*var input = new[]
            {
                "C)D",
                "COM)B",
                "D)E",
                "M)N",
                "B)C",
                "F)O",
                "N)SAN",
                "D)I",
                "L)N",
                "E)J",
                "C)M",
                "K)YOU",
                "E)F",
                "N)I",
                "G)H",
                "F)P",
                "B)G",
                "J)K",
                "K)L"
            }; // 61*/

            /*var input = new[]
            {
                "COM)B",
                "B)C",
                "C)D",
                "D)E",
                "E)F",
                "B)G",
                "L)I",
                "G)H",
                "D)I",
                "E)J",
                "J)K",
                "K)L",                
                "K)YOU",
                "I)SAN"
            };*/

            foreach (var line in input)
            {
                AddSpaceObject(line);
            }

            Operate(cache["YOU"].Orbits[0], 0, "YOU");

            var shortestDistance = pathPairs.Where(x => x.Name == cache["SAN"].Orbits[0].Name).Min(x => x.Transfers);

            Console.WriteLine($"Transfers: { shortestDistance }");
            Console.ReadLine();
        }

        private static void AddSpaceObject(string line)
        {
            var splitted = line.Split(')');
            var left = splitted[0];
            var right = splitted[1];

            TryAdd(left);
            TryAdd(right);

            cache[right].Orbits.Add(cache[left]);
            cache[left].OrbitedBy.Add(cache[right]);
        }

        private static void TryAdd(string name)
        {
            if (cache.ContainsKey(name))
            {
                return;
            }

            cache.Add(name, new SpaceObject(name));
        }

        private static void Operate(SpaceObject spaceObject, int transfers, string except)
        {
            pathPairs.Add(new PathPair { Name = spaceObject.Name, Transfers = transfers });

            foreach (var orbit in 
                spaceObject.Orbits.Where(x => x.Name != except && !IsInExceptionOrbitsPairs(spaceObject, x)))
            {
                exceptionPairs.Add(new ExceptionPair(spaceObject.Name, orbit.Name));

                //LogOpearation(spaceObject, orbit);
                Operate(orbit, transfers + 1, spaceObject.Name);
            }

            foreach (var orbit in spaceObject.OrbitedBy.Where(x => x.Name != except && !IsInExceptionOrbitedByPairs(spaceObject, x)))
            {
                exceptionPairs.Add(new ExceptionPair(spaceObject.Name, orbit.Name));

                //LogOpearation(spaceObject, orbit);
                Operate(orbit, transfers + 1, spaceObject.Name);
            }
        }

        private static bool IsInExceptionOrbitsPairs(SpaceObject firstObject, SpaceObject secondObject)
        {
            var x = exceptionPairs.Contains(new ExceptionPair(firstObject.Name, secondObject.Name));
            return x;
        }

        private static bool IsInExceptionOrbitedByPairs(SpaceObject firstObject, SpaceObject secondObject)
        {
            var x = exceptionPairs.Contains(new ExceptionPair(firstObject.Name, secondObject.Name));
            return x;
        }

        private static void LogOpearation(SpaceObject x, SpaceObject y)
        {
            Console.WriteLine($"{ x.Name } -> { y.Name }");
        }
    }

    class SpaceObject
    {
        public string Name { get; set; }
        public List<SpaceObject> Orbits { get; set; } = new List<SpaceObject>();
        public List<SpaceObject> OrbitedBy { get; set; } = new List<SpaceObject>();

        public SpaceObject(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Object { Name }";
        }
    }

    class PathPair
    {
        public string Name { get; set; }
        public int Transfers { get; set; }

        public override string ToString()
        {
            return $"[ { Name } ] = { Transfers }";
        }
    }

    struct ExceptionPair
    {
        public string ObjectFirst;
        public string ObjectSecond;

        public ExceptionPair(string x, string y)
        {
            ObjectFirst = x;
            ObjectSecond = y;
        }

        public override string ToString()
        {
            return $" [{ ObjectFirst }] -> [{ ObjectSecond }] ";
        }
    }
}
