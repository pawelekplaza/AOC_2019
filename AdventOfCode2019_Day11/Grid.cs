using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day11
{
    public class Grid : List<Square>
    {
        public Square GetSquare(int x, int y)
        {
            var result = this.FirstOrDefault(square => square.X == x && square.Y == y);

            if (result == null)
            {
                result = new Square(Color.Black, x, y);
                Add(result);
            }

            return result;
        }
    }

    public class Square
    {
        public Color Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int PaintingCounter { get; set; }
        public int HeadingDirection { get; set; } = 1;

        // 0 - Left
        // 1 - Up
        // 2 - Right
        // 3 - Down

        public Square()
        {

        }

        public Square(Color color, int x, int y)
        {
            Color = color;
            X = x;
            Y = y;
        }

        public NewPosition GetPositionAfterTurn(int direction)
        {
            switch (HeadingDirection)
            {
                case 0:
                    return direction == 0 ? new NewPosition(X, Y + 1, 3) : new NewPosition(X, Y - 1, 1);
                case 1:
                    return direction == 0 ? new NewPosition(X - 1, Y, 0) : new NewPosition(X + 1, Y, 2);
                case 2:
                    return direction == 0 ? new NewPosition(X, Y - 1, 1) : new NewPosition(X, Y + 1, 3);
                case 3:
                    return direction == 0 ? new NewPosition(X + 1, Y, 2) : new NewPosition(X - 1, Y, 0);
                default:
                    throw new Exception("Invalid heading direction.");
            }
        }
    }

    public class NewPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int HeadingDirection { get; set; }

        public NewPosition(int x, int y, int headingDirection)
        {
            X = x;
            Y = y;
            HeadingDirection = headingDirection;
        }
    }

    public enum Color
    {
        Black,
        White
    }

    public static class Extensions
    {
        public static int ToInt(this Color color)
        {
            return (int)color;
        }

        public static BigInteger ToBigInt(this Color color)
        {
            return (int)color;
        }

        public static Color ToColor(this BigInteger value)
        {
            return value == 0 ? Color.Black : Color.White;
        }
    }
}
