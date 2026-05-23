using System;

namespace Legacy.Time
{
    public sealed class WorldClock
    {
        public GameDateTime Now { get; private set; }
        public bool IsPaused { get; set; }
        public TimeBand CurrentBand => GetTimeBand(Now.Time);

        public event Action<TimeOfDay> MinuteTicked;
        public event Action<TimeOfDay> HourTicked;
        public event Action<GameDate> DayStarted;
        public event Action<TimeBand> TimeBandChanged;

        public WorldClock(GameDateTime initialTime)
        {
            Now = initialTime;
        }

        public void AdvanceMinutes(int minutes)
        {
            if (minutes < 0) {
                throw new ArgumentOutOfRangeException(nameof(minutes), "Minutes must not be negative.");
            }

            if (IsPaused || minutes == 0) {
                return;
            }

            for (int i = 0; i < minutes; i++) {
                GameDate previousDate = Now.Date;
                TimeBand previousBand = CurrentBand;
                Now = Now.AddMinutes(1);

                MinuteTicked?.Invoke(Now.Time);

                if (Now.Time.Minute == 0) {
                    HourTicked?.Invoke(Now.Time);
                }

                if (!Now.Date.Equals(previousDate)) {
                    DayStarted?.Invoke(Now.Date);
                }

                if (CurrentBand != previousBand) {
                    TimeBandChanged?.Invoke(CurrentBand);
                }
            }
        }

        public void SetTime(GameDateTime time)
        {
            TimeBand previousBand = CurrentBand;
            Now = time;

            if (CurrentBand != previousBand) {
                TimeBandChanged?.Invoke(CurrentBand);
            }
        }

        public static TimeBand GetTimeBand(TimeOfDay time)
        {
            int minutes = time.TotalMinutes;
            if (minutes >= 240 && minutes < 420) {
                return TimeBand.Dawn;
            }

            if (minutes >= 420 && minutes < 660) {
                return TimeBand.Morning;
            }

            if (minutes >= 660 && minutes < 840) {
                return TimeBand.Midday;
            }

            if (minutes >= 840 && minutes < 1020) {
                return TimeBand.Afternoon;
            }

            if (minutes >= 1020 && minutes < 1140) {
                return TimeBand.Dusk;
            }

            if (minutes >= 1140 && minutes < 1320) {
                return TimeBand.Evening;
            }

            return TimeBand.Night;
        }
    }
}
