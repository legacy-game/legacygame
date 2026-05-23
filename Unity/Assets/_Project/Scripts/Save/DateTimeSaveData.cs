using System;

namespace Legacy.Save
{
    [Serializable]
    public sealed class DateTimeSaveData
    {
        public int year;
        public int month;
        public int day;
        public int absoluteDay;
        public int hour;
        public int minute;
    }
}
