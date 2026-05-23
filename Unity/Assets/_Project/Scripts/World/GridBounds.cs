using System;

namespace Legacy.World
{
    [Serializable]
    public readonly struct GridBounds : IEquatable<GridBounds>
    {
        public GridCoord Min { get; }
        public int Width { get; }
        public int Height { get; }

        public GridBounds(GridCoord min, int width, int height)
        {
            if (width <= 0) {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");
            }

            if (height <= 0) {
                throw new ArgumentOutOfRangeException(nameof(height), "Height must be positive.");
            }

            Min = min;
            Width = width;
            Height = height;
        }

        public bool Contains(GridCoord coord)
        {
            return coord.X >= Min.X
                && coord.Y >= Min.Y
                && coord.X < Min.X + Width
                && coord.Y < Min.Y + Height;
        }

        public bool Equals(GridBounds other)
        {
            return Min == other.Min && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is GridBounds other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked {
                int hash = Min.GetHashCode();
                hash = (hash * 397) ^ Width;
                hash = (hash * 397) ^ Height;
                return hash;
            }
        }
    }
}
