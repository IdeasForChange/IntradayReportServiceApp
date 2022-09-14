using IntradayReportRunner.Utilities;
using Services;

namespace IntradayReportingService.Tests
{
    [TestClass]
    public class TradeAggregatorTests
    {
        public DateTime TradeDate { get; internal set; }

        public int PowerPeriodCount { get; internal set; }

        [TestInitialize]
        public void Initialise()
        {
            TradeDate = DateTime.Now;
            PowerPeriodCount = 24;
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void AggregateIntradayTrade_UnderTest_ReturnsCorrectData()
        {
            // ARRANGE
            var tradeAggregator = new TradeAggregator();
            var expectedResult = GetExpectedIntradayTradeAggregationResult();

            // ACT
            var actuals = tradeAggregator.AggregateIntradayTrade(GetDeterministicIntradayTradeData());

            // ASSERT
            Assert.IsNotNull(actuals);

            // Check if sequence are same
            Assert.IsTrue(Enumerable.SequenceEqual<PowerPeriod>(expectedResult.Periods, actuals.Periods));
        }

        public IEnumerable<PowerTrade> GetDeterministicIntradayTradeData()
        {
            // First sample example (ALL 100)
            var powerTrade1 = PowerTrade.Create(TradeDate, PowerPeriodCount);
            for(int arrayIndex=0; arrayIndex < PowerPeriodCount; arrayIndex++)
            {
                powerTrade1.Periods[arrayIndex].Volume = 100;
            }

            // Second Power Trade (First 11 = 50, rest = -20
            var powerTrade2 = PowerTrade.Create(TradeDate, PowerPeriodCount);
            for (int arrayIndex=0; arrayIndex < 11; arrayIndex++)
            {
                powerTrade2.Periods[arrayIndex].Volume = 50;
            }
            for (int arrayIndex = 11; arrayIndex < 24; arrayIndex++)
            {
                powerTrade2.Periods[arrayIndex].Volume = -20;
            }
            return new List<PowerTrade>() { powerTrade1, powerTrade2 };
        }

        public PowerTrade GetExpectedIntradayTradeAggregationResult()
        {
            // First sample example (ALL 100)
            var powerTrade = PowerTrade.Create(TradeDate, PowerPeriodCount);
            for (int arrayIndex = 0; arrayIndex < 11; arrayIndex++)
            {
                powerTrade.Periods[arrayIndex].Volume = 150;
            }
            for (int arrayIndex = 11; arrayIndex < 24; arrayIndex++)
            {
                powerTrade.Periods[arrayIndex].Volume = 80;
            }
            return powerTrade;
        }
    }
}
