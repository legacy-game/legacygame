using System;

namespace Legacy.World
{
    [Serializable]
    public readonly struct GridCoord : IEquatable<GridCoord>
    {
        public int X { get; }
        public int Y { get; }

        public GridCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int ManhattanDistanceTo(GridCoord other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public bool Equals(GridCoord other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCoord other && Equals(other);
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

        public static bool operator ==(GridCoord left, GridCoord right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridCoord left, GridCoord right)
        {
            return !left.Equals(right);
        }
    }
}
