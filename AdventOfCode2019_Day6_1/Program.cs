using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day6_1
{
    class Program
    {
        private static Dictionary<string, SpaceObject> cache = new Dictionary<string, SpaceObject>();
        private static Dictionary<string, int> totalOrbitsCache = new Dictionary<string, int>();
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
                "D)I",
                "E)J",
                "C)M",
                "E)F",
                "G)H",
                "F)P",
                "B)G",
                "J)K",
                "K)L"
            }; // 61*/

            foreach (var line in input)
            {
                AddSpaceObject(line);
            }

            var totalOrbits = cache.Sum(spaceObject => CountTotalOrbits(spaceObject.Value));

            Console.WriteLine($"Cache has { cache.Count } objects.");
            Console.WriteLine($"Total orbits: { totalOrbits }");
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
        }

        private static void TryAdd(string name)
        {
            if (cache.ContainsKey(name))
            {
                return;
            }

            cache.Add(name, new SpaceObject(name));
        }

        private static int CountTotalOrbits(SpaceObject spaceObject)
        {
            if (totalOrbitsCache.ContainsKey(spaceObject.Name))
            {
                return totalOrbitsCache[spaceObject.Name];
            }

            if (spaceObject.Orbits.Count == 0)
            {
                return 0;
            }

            var totalOrbits = spaceObject.Orbits.Count + spaceObject.Orbits.Sum(CountTotalOrbits);

            if (!totalOrbitsCache.ContainsKey(spaceObject.Name))
            {
                totalOrbitsCache.Add(spaceObject.Name, totalOrbits);
            }

            return totalOrbits;
        }
    }

    class SpaceObject
    {
        public string Name { get; set; }
        public List<SpaceObject> Orbits { get; set; } = new List<SpaceObject>();

        public SpaceObject(string name)
        {
            Name = name;
        }
    }
}
