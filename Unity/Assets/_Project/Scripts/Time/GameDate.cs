using System;

namespace Legacy.Time
{
    [Serializable]
    public readonly struct GameDate : IEquatable<GameDate>
    {
        private const int DaysPerMonth = 30;
        private const int MonthsPerYear = 12;
        private static readonly string[] DayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public int AbsoluteDay { get; }
        public string DayOfWeek => DayNames[AbsoluteDay % DayNames.Length];

        public GameDate(int year, int month, int day, int absoluteDay = 0)
        {
            if (month < 1 || month > MonthsPerYear) {
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be 1..12.");
            }

            if (day < 1 || day > DaysPerMonth) {
                throw new ArgumentOutOfRangeException(nameof(day), "Day must be 1..30.");
            }

            if (absoluteDay < 0) {
                throw new ArgumentOutOfRangeException(nameof(absoluteDay), "Absolute day must not be negative.");
            }

            Year = year;
            Month = month;
            Day = day;
            AbsoluteDay = absoluteDay;
        }

        public GameDate AddDays(int days)
        {
            int zeroBasedDay = (Month - 1) * DaysPerMonth + (Day - 1) + days;
            int year = Year + Math.DivRem(zeroBasedDay, DaysPerMonth * MonthsPerYear, out int yearRemainder);

            if (yearRemainder < 0) {
                year--;
                yearRemainder += DaysPerMonth * MonthsPerYear;
            }

            int month = (yearRemainder / DaysPerMonth) + 1;
            int day = (yearRemainder % DaysPerMonth) + 1;
            return new GameDate(year, month, day, AbsoluteDay + days);
        }

        public bool Equals(GameDate other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day && AbsoluteDay == other.AbsoluteDay;
        }

        public override bool Equals(object obj)
        {
            return obj is GameDate other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked {
                int hash = Year;
                hash = (hash * 397) ^ Month;
                hash = (hash * 397) ^ Day;
                hash = (hash * 397) ^ AbsoluteDay;
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{Year:0000}-{Month:00}-{Day:00}";
        }
    }
}
