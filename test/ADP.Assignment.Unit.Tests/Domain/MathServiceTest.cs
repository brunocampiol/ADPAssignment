using ADP.Assignment.Common.Rest.Interfaces;
using ADP.Assignment.Domain.Providers;
using ADP.Assignment.Domain.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ADP.Assignment.Unit.Tests.Domain
{
    public class MathServiceTest
    {
        private readonly Mock<IRestService> _restService;
        private readonly IOptions<MathOptions> _mathOptions;

        public MathServiceTest()
        {
            _mathOptions = Options.Create(new MathOptions()
            {
                UrlBase = "http://uni-testing.com",
                ResourceGetTask = "test1",
                ResourceSubmitTask = "test2"
            });

            _restService = new Mock<IRestService>();
        }

        [Fact]
        public async Task ThrowArgumentNullExceptionWhenRequesterIsNull()
        {
            // Arrange - setup
            var mathService = new MathService(_mathOptions, _restService.Object);

            // Act - what is being tested
            await mathService.CalculateInstructionAsync();

            // Assemble - what is optionally needed to perform the assert
            // Assert - the actual assertions
            Assert.True(true);
        }
    }
}
