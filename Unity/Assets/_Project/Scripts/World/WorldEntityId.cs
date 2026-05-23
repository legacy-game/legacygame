using System;

namespace Legacy.World
{
    [Serializable]
    public readonly struct WorldEntityId : IEquatable<WorldEntityId>
    {
        public string Value { get; }

        public WorldEntityId(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException("Entity id must not be empty.", nameof(value));
            }

            Value = value;
        }

        public bool Equals(WorldEntityId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is WorldEntityId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(WorldEntityId left, WorldEntityId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WorldEntityId left, WorldEntityId right)
        {
            return !left.Equals(right);
        }
    }
}
