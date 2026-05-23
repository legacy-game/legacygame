using System;

namespace Legacy.Time
{
    [Serializable]
    public readonly struct TimeOfDay : IEquatable<TimeOfDay>, IComparable<TimeOfDay>
    {
        private const int MinutesPerDay = 1440;

        public int Hour { get; }
        public int Minute { get; }
        public int TotalMinutes => Hour * 60 + Minute;

        public TimeOfDay(int hour, int minute)
        {
            if (hour < 0 || hour > 23) {
                throw new ArgumentOutOfRangeException(nameof(hour), "Hour must be 0..23.");
            }

            if (minute < 0 || minute > 59) {
                throw new ArgumentOutOfRangeException(nameof(minute), "Minute must be 0..59.");
            }

            Hour = hour;
            Minute = minute;
        }

        public TimeOfDay Add(int minutes)
        {
            int total = ((TotalMinutes + minutes) % MinutesPerDay + MinutesPerDay) % MinutesPerDay;
            return new TimeOfDay(total / 60, total % 60);
        }

        public int CompareTo(TimeOfDay other)
        {
            return TotalMinutes.CompareTo(other.TotalMinutes);
        }

        public bool Equals(TimeOfDay other)
        {
            return Hour == other.Hour && Minute == other.Minute;
        }

        public override bool Equals(object obj)
        {
            return obj is TimeOfDay other && Equals(other);
        }

        public override int GetHashCode()
        {
            return TotalMinutes;
        }

        public override string ToString()
        {
            return $"{Hour:00}:{Minute:00}";
        }
    }
}
