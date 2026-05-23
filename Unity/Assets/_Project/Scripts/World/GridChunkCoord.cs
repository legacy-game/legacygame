using System;

namespace Legacy.World
{
    [Serializable]
    public readonly struct GridChunkCoord : IEquatable<GridChunkCoord>
    {
        public const int Size = 16;

        public int X { get; }
        public int Y { get; }

        public GridChunkCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static GridChunkCoord FromGridCoord(GridCoord coord)
        {
            return new GridChunkCoord(FloorDiv(coord.X, Size), FloorDiv(coord.Y, Size));
        }

        public bool Equals(GridChunkCoord other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridChunkCoord other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public static bool operator ==(GridChunkCoord left, GridChunkCoord right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridChunkCoord left, GridChunkCoord right)
        {
            return !left.Equals(right);
        }

        private static int FloorDiv(int value, int divisor)
        {
            int result = value / divisor;
            int remainder = value % divisor;

            if (remainder != 0 && ((remainder > 0) != (divisor > 0))) {
                result--;
            }

            return result;
        }
    }
}
