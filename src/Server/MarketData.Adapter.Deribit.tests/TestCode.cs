using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MarketData.Adapter.Deribit.tests
{
    public class TestCode
    {
        [Test]
        public void Test_Epoch_Convertion_Algo()
        {
            Assert.That(FromUnixTime(1590476708320), Is.EqualTo(new DateTime(2020, 05,26,7,5,8,320, DateTimeKind.Utc)));
        }
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddMilliseconds(unixTime);
        }
    }
}