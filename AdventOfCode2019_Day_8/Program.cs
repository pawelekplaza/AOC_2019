using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day_8
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = "123456789012";
            var input = File.ReadAllLines("input.txt")[0];

            var imageReader = new ImageReader(input);
            var layers = imageReader.GetLayers(25, 6);

            var colors = new List<Color>();
            for (int i = 0; i < layers[0].Digits.Count; i++)
            {
                colors.Add((Color) GetFirstNonTransparentDigit(layers, i));
            }

            Console.BackgroundColor = ConsoleColor.Blue;

            int position = 0;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    switch (colors[position])
                    {
                        case Color.Black:
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("*");
                            break;
                        case Color.White:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("X");
                            break;
                        case Color.Transparent:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                    }
                    
                    position++;
                }

                Console.WriteLine();
            }
            Console.ReadLine();
        }

        static int GetFirstNonTransparentDigit(List<Layer> layers, int position)
        {
            var color = 2;
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].Digits[position] != 2)
                {
                    color = layers[i].Digits[position];
                    break;
                }
            }

            return color;
        }
    }

    class ImageReader
    {
        private List<int> digits;

        public ImageReader()
        {

        }

        public ImageReader(string input)
        {
            Initialize(input);
        }
        public void Initialize(string input)
        {
            digits = input.Select(x => int.Parse(x.ToString())).ToList();
        }

        public List<Layer> GetLayers(int width, int height)
        {
            var digitsPerLayer = width * height;
            var layers = new List<Layer>();

            for (int i = 0; i < digits.Count / digitsPerLayer; i++)
            {
                layers.Add(new Layer(digits.Skip(digitsPerLayer * i).Take(digitsPerLayer)));
            }

            return layers;
        }
    }

    class Layer
    {
        public List<int> Digits { get; private set; }

        public Layer()
        {

        }

        public Layer(IEnumerable<int> input)
        {
            Initialize(input);
        }

        public void Initialize(IEnumerable<int> input)
        {
            Digits = new List<int>(input);
        }
    }

    enum Color
    {
        Black,
        White,
        Transparent
    }
}
