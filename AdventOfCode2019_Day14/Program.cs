using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = new[]
            //{
            //    "9 ORE => 2 A",
            //    "8 ORE => 3 B",
            //    "7 ORE => 5 C",
            //    "3 A, 4 B => 1 AB",
            //    "5 B, 7 C => 1 BC",
            //    "4 C, 1 A => 1 CA",
            //    "2 AB, 3 BC, 4 CA => 1 FUEL"
            //};

            //var input = new[]
            //{
            //    "157 ORE => 5 NZVS",
            //    "165 ORE => 6 DCFZ",
            //    "44 XJWVT, 5 KHKGT, 1 QDVJ, 29 NZVS, 9 GPVTF, 48 HKGWZ => 1 FUEL",
            //    "12 HKGWZ, 1 GPVTF, 8 PSHF => 9 QDVJ",
            //    "179 ORE => 7 PSHF",
            //    "177 ORE => 5 HKGWZ",
            //    "7 DCFZ, 7 PSHF => 2 XJWVT",
            //    "165 ORE => 2 GPVTF",
            //    "3 DCFZ, 7 NZVS, 5 HKGWZ, 10 PSHF => 8 KHKGT"
            //};

            //var input = new[]
            //{
            //    "2 VPVL, 7 FWMGM, 2 CXFTF, 11 MNCFX => 1 STKFG",
            //    "17 NVRVD, 3 JNWZP => 8 VPVL",
            //    "53 STKFG, 6 MNCFX, 46 VJHF, 81 HVMC, 68 CXFTF, 25 GNMV => 1 FUEL",
            //    "22 VJHF, 37 MNCFX => 5 FWMGM",
            //    "139 ORE => 4 NVRVD",
            //    "144 ORE => 7 JNWZP",
            //    "5 MNCFX, 7 RFSQX, 2 FWMGM, 2 VPVL, 19 CXFTF => 3 HVMC",
            //    "5 VJHF, 7 MNCFX, 9 VPVL, 37 CXFTF => 6 GNMV",
            //    "145 ORE => 6 MNCFX",
            //    "1 NVRVD => 8 CXFTF",
            //    "1 VJHF, 6 MNCFX => 4 RFSQX",
            //    "176 ORE => 6 VJHF"
            //};

            var input = new[]
            {
                "171 ORE => 8 CNZTR",
                "7 ZLQW, 3 BMBT, 9 XCVML, 26 XMNCP, 1 WPTQ, 2 MZWV, 1 RJRHP => 4 PLWSL",
                "114 ORE => 4 BHXH",
                "14 VRPVC => 6 BMBT",
                "6 BHXH, 18 KTJDG, 12 WPTQ, 7 PLWSL, 31 FHTLT, 37 ZDVW => 1 FUEL",
                "6 WPTQ, 2 BMBT, 8 ZLQW, 18 KTJDG, 1 XMNCP, 6 MZWV, 1 RJRHP => 6 FHTLT",
                "15 XDBXC, 2 LTCX, 1 VRPVC => 6 ZLQW",
                "13 WPTQ, 10 LTCX, 3 RJRHP, 14 XMNCP, 2 MZWV, 1 ZLQW => 1 ZDVW",
                "5 BMBT => 4 WPTQ",
                "189 ORE => 9 KTJDG",
                "1 MZWV, 17 XDBXC, 3 XCVML => 2 XMNCP",
                "12 VRPVC, 27 CNZTR => 2 XDBXC",
                "15 KTJDG, 12 BHXH => 5 XCVML",
                "3 BHXH, 2 VRPVC => 7 MZWV",
                "121 ORE => 7 VRPVC",
                "7 XCVML => 6 RJRHP",
                "5 BHXH, 4 VRPVC => 5 LTCX"
            };

            //var input = File.ReadAllLines("input.txt");

            var elements = GetElements(input);

            Console.WriteLine($"ORE needed: {elements.GetTotalOreCost(elements.GetDependencies("FUEL"))}");
            

            Console.ReadLine();
        }

        static List<Element> GetElements(string[] input)
        {
            var elements = new List<Element>();
            foreach (var line in input)
            {
                var element = new Element();
                var splitted = line.Split('=');
                var ingredients = splitted[0].Split(',');
                element.Dependencies = new List<Dependency>(ingredients.Select(x =>
                {
                    var ingredient = x.Trim().Split();
                    return new Dependency(ingredient[1], int.Parse(ingredient[0]));
                }));

                var result = splitted[1].Replace(">", "").Trim().Split();

                element.Count = int.Parse(result[0]);
                element.Name = result[1];

                elements.Add(element);
            }

            return elements;
        }
    }

    public class Element
    {
        public int Count { get; set; }
        public List<Dependency> Dependencies { get; set; }
        public string Name { get; set; }
    }

    public class Dependency
    {
        public int Count { get; set; }
        public string Name { get; set; }

        public Dependency(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public override string ToString()
        {
            return $"{ Count } { Name }";
        }
    }

    public static class Extensions
    {
        public static Element GetElement(this List<Element> elements, string name)
        {
            return elements.FirstOrDefault(x => x.Name == name);
        }

        public static List<Dependency> GetDependencies(this List<Element> elements, string name)
        {
            return GetElement(elements, name).Dependencies;
        }

        /*public static int GetTotalOreCost(this List<Element> elements, List<Dependency> dependencies)
        {
            if (dependencies.Count == 1 && dependencies[0].Name == "ORE")
            {
                return dependencies[0].Count;
            }

            var tempDependencies = new List<Dependency>();
            foreach (var dependency in dependencies)
            {
                var dependentElement = elements.GetElement(dependency.Name);
                if (dependentElement.Dependencies.Count == 1)
                {
                    tempDependencies.Add(dependency);
                    continue;
                }

                var amountNeeded = GetNeeded(dependency.Count, dependentElement.Count);
                tempDependencies.Add(new Dependency { Name = dependency.Name, Count = amountNeeded });
            }

            var nextDependencies = new List<Dependency>();
            foreach (var dependency in tempDependencies)
            {
                var dependentElement = elements.GetElement(dependency.Name);
                if (dependentElement.Dependencies.Count == 1 && dependentElement.Dependencies[0].Name == "ORE")
                {
                    continue;
                }

                var times = dependency.Count / dependentElement.Count;
                var nextDeps = dependentElement.Dependencies.Select(x => new Dependency {Name = x.Name, Count = x.Count}).ToList();

                foreach (var nextDep in nextDeps)
                {
                    nextDep.Count *= times;
                }

                nextDependencies.AddRange(nextDeps);
            }

            var currentGrouped = dependencies.GroupBy(x => x.Name);
            var currentDependencies = currentGrouped.Select(x => new Dependency { Name = x.Key, Count = x.Sum(y => y.Count) }).ToList();
            currentDependencies.RemoveAll(x =>
                elements.GetElement(x.Name).Dependencies.Count == 1 &&
                elements.GetElement(x.Name).Dependencies[0].Name != "ORE");

            var grouped = nextDependencies.GroupBy(x => x.Name);
            nextDependencies = grouped.Select(x => new Dependency { Name = x.Key, Count = x.Sum(y => y.Count) }).ToList();

            if (nextDependencies.Count == 0)
            {
                int total = 0;
                foreach (var element in currentDependencies)
                {
                    var ele = elements.GetElement(element.Name);
                    var times = GetNeeded(element.Count, ele.Count) / ele.Count;
                    total += ele.Dependencies[0].Count * times;
                }

                return total;
            }

            foreach (var nextDep in nextDependencies)
            {
                var diff = currentDependencies.FirstOrDefault(x => x.Name == nextDep.Name);
                if (diff != null)
                {
                    nextDep.Count += diff.Count;
                }
            }

            foreach (var currentDep in currentDependencies)
            {
                var currentEle = elements.GetElement(currentDep.Name);
                if (currentEle.Dependencies.Count == 1 && !nextDependencies.Any(x => x.Name == currentDep.Name))
                {
                    //nextDependencies.Add(currentDep);
                }
            }

            return GetTotalOreCost(elements, nextDependencies);
        }*/

        public static int GetTotalOreCost(this List<Element> elements, List<Dependency> dependencies)
        {
            var allDependencies = new List<Dependency>();

            foreach (var dependency in dependencies)
            {
                if (dependency.Name == "ORE")
                {
                    allDependencies.Add(dependency);
                    continue;
                }

                var depElement = elements.GetElement(dependency.Name);

                if (depElement.Dependencies[0].Name == "ORE")
                {
                    allDependencies.Add(dependency);
                    continue;
                }

                var amountNeeded = GetRealNeeded(dependency.Count, depElement.Count);
                var times = amountNeeded / depElement.Count;
                
                allDependencies.AddRange(depElement.Dependencies.Select(x => new Dependency(x.Name, x.Count * times)));
            }

            allDependencies = allDependencies.GroupBy(x => x.Name).Select(x => new Dependency(x.Key, x.Sum(y => y.Count))).ToList();

            if (allDependencies.All(x => elements.GetElement(x.Name).Dependencies[0].Name == "ORE"))
            {
                return GetOreCost(elements, allDependencies);
            }

            return elements.GetTotalOreCost(allDependencies);
        }

        private static int GetOreCost(List<Element> elements, List<Dependency> allDependencies)
        {
            int totalCost = 0;
            foreach (var dependency in allDependencies)
            {
                var element = elements.GetElement(dependency.Name);
                var times = GetRealNeeded(dependency.Count, element.Count) / element.Count;
                totalCost += element.Dependencies[0].Count * times;
            }

            return totalCost;
        }

        private static int GetRealNeeded(int dependencyAmount, int elementAmount)
        {
            if (dependencyAmount % elementAmount == 0)
            {
                return dependencyAmount;
            }

            int i = elementAmount;
            for (; i < dependencyAmount; i += elementAmount)
            {

            }

            return i;
        }

        private static double GetNeeded(double dependencyAmount, int elementAmount)
        {
            return dependencyAmount / elementAmount;
        }
    }
}
