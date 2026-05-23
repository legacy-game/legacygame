using Legacy.World;
using NUnit.Framework;

namespace Legacy.Tests.EditMode
{
    public sealed class GridCoordTests
    {
        [Test]
        public void EqualCoords_HaveSameHash()
        {
            var a = new GridCoord(10, 20);
            var b = new GridCoord(10, 20);

            Assert.That(a, Is.EqualTo(b));
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void ManhattanDistance_ReturnsGridDistance()
        {
            var a = new GridCoord(2, 3);
            var b = new GridCoord(7, 1);

            Assert.That(a.ManhattanDistanceTo(b), Is.EqualTo(7));
        }
    }
}
