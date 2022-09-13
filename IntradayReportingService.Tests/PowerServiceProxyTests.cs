using IntradayReportService;
using Microsoft.Extensions.Logging;
using Moq;
using Services;

namespace IntradayReportingService.Tests
{
    [TestClass]
    public class PowerServiceProxyTests
    {
        [TestMethod]
        public async Task PowerServiceProxy_WhenGetDataAsync_IsCalled_ReturnsNotNullData()
        {
            // ARRANGE
            var today = DateTime.Now;
            var logger = new Mock<ILogger<PowerServiceProxy>>();
            var powerService = new Mock<IPowerService>();

            // Setup expected output
            powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>()))
                .Returns(Task.FromResult(GetData()));

            // Initialise the class under test 
            var service = new PowerServiceProxy(powerService.Object, logger.Object);

            // ACT
            var data = await service.GetTradesAsync(today);

            // ASSERT
            Assert.IsNotNull(data);
        }

        private static IEnumerable<PowerTrade> GetData()
        {
            return new List<PowerTrade>()
            {

            };
        }
    }
}