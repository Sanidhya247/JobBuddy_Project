using System.IO;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using job_buddy_backend.Controllers;
using job_buddy_backend.Core;
using Microsoft.Extensions.Logging;

namespace job_buddy_backend.Tests
{
    public class AtsScoringControllerTests
    {
        private readonly Mock<AtsScoringService> _mockService;
        private readonly AtsScoringController _controller;

        public AtsScoringControllerTests()
        {
            _mockService = new Mock<AtsScoringService>(Mock.Of<ILogger<AtsScoringService>>());
            _controller = new AtsScoringController(_mockService.Object);
        }

        //[Fact]
        //public async Task GetAtsScore_ShouldReturnOk_WhenValidInputsProvided()
        //{
        //    // Arrange
        //    var mockFile = new Mock<IFormFile>();
        //    mockFile.Setup(f => f.FileName).Returns("resume.pdf");
        //    mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
        //        .Callback<Stream, CancellationToken>((stream, token) =>
        //        {
        //            using var writer = new StreamWriter(stream);
        //            writer.Write("Sample resume text");
        //            writer.Flush();
        //        })
        //        .Returns(Task.CompletedTask);

        //    var request = new AtsScoringRequest
        //    {
        //        JobDescription = "Sample job description",
        //        Resume = mockFile.Object
        //    };

        //    _mockService.Setup(s => s.CalculateAtsScore(It.IsAny<string>(), It.IsAny<string>())).Returns(85.5);

        //    // Act
        //    var result = await _controller.GetAtsScore(request);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var response = Assert.IsAssignableFrom<dynamic>(okResult.Value);
        //    response.Score.Should().Be(85.5);
        //}

        [Fact]
        public async Task GetAtsScore_ShouldReturnBadRequest_WhenInputsAreInvalid()
        {
            // Arrange
            var request = new AtsScoringRequest
            {
                JobDescription = string.Empty, // Missing Job Description
                Resume = null                  // Missing Resume
            };

            // Act
            var result = await _controller.GetAtsScore(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Resume and Job Description cannot be empty.");
        }

        //[Fact]
        //public async Task GetAtsScore_ShouldReturnBadRequest_WhenServiceThrowsException()
        //{
        //    // Arrange
        //    var mockFile = new Mock<IFormFile>();
        //    mockFile.Setup(f => f.FileName).Returns("resume.pdf");
        //    mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).Returns(Task.CompletedTask);

        //    var request = new AtsScoringRequest
        //    {
        //        JobDescription = "Sample job description",
        //        Resume = mockFile.Object
        //    };

        //    _mockService.Setup(s => s.CalculateAtsScore(It.IsAny<string>(), It.IsAny<string>()))
        //        .Throws(new InvalidOperationException("Error calculating score"));

        //    // Act
        //    var result = await _controller.GetAtsScore(request);

        //    // Assert
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        //    badRequestResult.Value.Should().Be("Error calculating score");
        //}
    }
}
