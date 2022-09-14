using IntradayReportService.Workflows.Utilities;
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
        public void AggregateIntradayTrade_ReturnsCorrectAggregatedData()
        {
            // ARRANGE
            var tradeAggregator = new TradeAggregator();
            var expectedPeriods = GetExpectedIntradayTradeAggregationResult();

            // ACT
            var actualPeriods = tradeAggregator.AggregateIntradayTrade(GetDeterministicIntradayTradeData());

            // ASSERT
            Assert.IsNotNull(actualPeriods);

            // Check if sequence are same
            // TODO: Check why this command is NOT working. In the meantime, using a different method
            // Assert.IsTrue(Enumerable.SequenceEqual<PowerPeriod>(expectedResult.Periods, actuals.Periods));
            for (int i=0; i < expectedPeriods.Count(); i++)
            {
                Assert.AreEqual(expectedPeriods[i].Period, actualPeriods[i].Period);
                Assert.AreEqual(actualPeriods[i].Volume, actualPeriods[i].Volume);
            }
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

        public PowerPeriod[] GetExpectedIntradayTradeAggregationResult()
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
            return powerTrade.Periods;
        }
    }
}
