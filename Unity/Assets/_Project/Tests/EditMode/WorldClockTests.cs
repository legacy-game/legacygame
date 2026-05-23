using Legacy.Time;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class WorldClockTests
    {
        [Test]
        public void TimeOfDayAdd_WrapsAcrossMidnight()
        {
            var time = new TimeOfDay(23, 50);

            Assert.That(time.Add(15), Is.EqualTo(new TimeOfDay(0, 5)));
        }

        [Test]
        public void GameDateTimeAddMinutes_AdvancesDate()
        {
            var dateTime = new GameDateTime(new GameDate(2003, 5, 14), new TimeOfDay(23, 59));

            GameDateTime next = dateTime.AddMinutes(1);

            Assert.That(next.Date.Day, Is.EqualTo(15));
            Assert.That(next.Time, Is.EqualTo(new TimeOfDay(0, 0)));
        }
    }
}
