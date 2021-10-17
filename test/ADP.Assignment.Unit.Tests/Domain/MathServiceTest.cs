using ADP.Assignment.Common.Extensions;
using ADP.Assignment.Common.Rest.Interfaces;
using ADP.Assignment.Domain.Models;
using ADP.Assignment.Domain.Providers;
using ADP.Assignment.Domain.Services;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
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
                ResourceGetTask = "api/test1",
                ResourceSubmitTask = "api/test2"
            });

            _restService = new Mock<IRestService>();
        }

        [Fact]
        public async Task WhenValidData_ExpectsNoErrors()
        {
            // Arrange - setup
            var mathOperation = new MathOperation()
            {
                Id = Guid.NewGuid(),
                Left = -10,
                Right = 10,
                Operation = "addition"
            };

            _restService.Setup(x => x.GetRequestAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.FromResult((mathOperation.ToJson(), HttpStatusCode.OK)));

            _restService.Setup(x => x.PostRequestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                        .Returns(Task.FromResult((mathOperation.ToJson(), HttpStatusCode.OK)));

            var mathService = new MathService(_mathOptions, _restService.Object);

            // Act - what is being tested
            var result = await mathService.CalculateInstructionAsync();

            // Assemble - what is optionally needed to perform the assert
            // Assert - the actual assertions
            Assert.NotNull(result);
        }

        [Fact]
        public async Task WhenDividingByZero_ExpectsDivideByZeroException()
        {
            // Arrange - setup
            var mathOperation = new MathOperation()
            {
                Id = Guid.NewGuid(),
                Left = 1,
                Right = 0,
                Operation = "division"
            };

            _restService.Setup(x => x.GetRequestAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(Task.FromResult((mathOperation.ToJson(), HttpStatusCode.OK)));

            var mathService = new MathService(_mathOptions, _restService.Object);

            // Act - what is being tested
            // Assemble - what is optionally needed to perform the assert
            // Assert - the actual assertions
            await Assert.ThrowsAsync<DivideByZeroException>(() => mathService.CalculateInstructionAsync());
        }
    }
}