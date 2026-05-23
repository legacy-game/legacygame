using System;

namespace Legacy.Time
{
    [Serializable]
    public readonly struct GameDateTime : IEquatable<GameDateTime>
    {
        public GameDate Date { get; }
        public TimeOfDay Time { get; }

        public GameDateTime(GameDate date, TimeOfDay time)
        {
            Date = date;
            Time = time;
        }

        public GameDateTime AddMinutes(int minutes)
        {
            int totalMinutes = Time.TotalMinutes + minutes;
            int dayDelta = Math.DivRem(totalMinutes, 1440, out int minuteOfDay);

            if (minuteOfDay < 0) {
                dayDelta--;
                minuteOfDay += 1440;
            }

            return new GameDateTime(Date.AddDays(dayDelta), new TimeOfDay(minuteOfDay / 60, minuteOfDay % 60));
        }

        public bool Equals(GameDateTime other)
        {
            return Date.Equals(other.Date) && Time.Equals(other.Time);
        }

        public override bool Equals(object obj)
        {
            return obj is GameDateTime other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked {
                return (Date.GetHashCode() * 397) ^ Time.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"{Date}T{Time}";
        }
    }
}
