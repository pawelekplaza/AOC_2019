using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019_Day13
{
    public class Grid : List<Tile>
    {
        public Tile GetTile(int x, int y)
        {
            var result = this.FirstOrDefault(square => square.X == x && square.Y == y);

            if (result == null)
            {
                result = new Tile(ObjectType.Empty, x, y);
                Add(result);
            }

            return result;
        }

        public Tile GetBall()
        {
            return this.FirstOrDefault(tile => tile.Type == ObjectType.Ball);
        }

        public Tile GetPaddle()
        {
            return this.FirstOrDefault(tile => tile.Type == ObjectType.Paddle);
        }
    }

    public class Tile
    {
        public ObjectType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tile()
        {

        }

        public Tile(ObjectType type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
        }

        public bool WillBounce()
        {
            return Type == ObjectType.Block || Type == ObjectType.Wall;
        }
    }

    public enum ObjectType
    {
        Empty,
        Wall,
        Block,
        Paddle,
        Ball
    }

    public static class Extensions
    {
        public static int ToInt(this ObjectType color)
        {
            return (int)color;
        }

        public static BigInteger ToBigInt(this ObjectType color)
        {
            return (int)color;
        }

        public static ObjectType ToObjectType(this BigInteger value)
        {
            return (ObjectType) ((int) value);
        }

        public static Move Reverse(this Move move)
        {
            return move == Move.Left ? Move.Right : Move.Left;
        }
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public enum Move
    {
        Left,        
        Right
    }
}
