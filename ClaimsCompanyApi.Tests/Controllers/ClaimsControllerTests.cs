using ClaimsCompanyApi.Controllers;
using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Handlers.Commands;
using ClaimsCompanyApi.Handlers.Queries;
using ClaimsCompanyApi.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClaimsCompanyApi.Tests.Controllers
{
    [Collection(TestRunSetup)]
    public class ClaimsControllerTests : TestBase
    {
        private readonly Mock<IMediator> _senderMock;
        private readonly ClaimsController _claimsController;

        public ClaimsControllerTests()
        {
            _senderMock = new Mock<IMediator>();
            _claimsController = new ClaimsController(_senderMock.Object);
        }

        [Fact]
        public async Task GetClaimById_WithValidId_ReturnsOkResult()
        {
            var query = new GetClaimByUcrQuery(FakeData.ClaimOne!.UCR);
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(FakeData.ClaimOne);
            
            var result = await _claimsController.GetClaimById(query.Ucr);
            var claim = GetClaimFromResultValue(result);

            result.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            claim.Should().Be(FakeData.ClaimOne);
            claim?.DaysOld.Should().Be(FakeData.ClaimOne.DaysOld);
        }
        
        [Fact]
        public async Task GetClaimById_WithInvalidId_ReturnsNotFoundResult()
        {
            const string claimId = "99";
            var query = new GetClaimByUcrQuery(claimId);
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync((Claim)null!);
            
            var result = await _claimsController.GetClaimById(claimId);
            
            result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task UpdateClaim_WithValidClaim_ReturnsOkResult()
        {
            var claimToUpdate = FakeData.ClaimOne;
            var command = new UpdateClaimCommand(claimToUpdate!);
            var expectedResult = new Claim
            {
                Id = claimToUpdate!.Id,
                UCR = claimToUpdate.UCR,
                CompanyId = claimToUpdate.CompanyId,
                ClaimDate = claimToUpdate.ClaimDate,
                LossDate = claimToUpdate.LossDate,
                AssuredName = claimToUpdate.AssuredName,
                IncurredLoss = 1050,
                Closed = claimToUpdate.Closed
            };
            _senderMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            
            var result = await _claimsController.UpdateClaim(claimToUpdate);
            var claim = GetClaimFromResultValue(result);

            result.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            claim?.DaysOld.Should().Be(FakeData.ClaimOne?.DaysOld);
            claim?.DaysOld.Should().Be(FakeData.ClaimOne?.DaysOld);
        }

        [Fact]
        public async Task UpdateClaim_WithInvalidClaim_ReturnsNotFoundResult()
        {
            var claim = new Claim { Id = 99, UCR = "Error"};
            var command = new UpdateClaimCommand(claim);
            _senderMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync((Claim)null!);
            
            var result = await _claimsController.UpdateClaim(claim);
            
            result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
        }

        private static Claim? GetClaimFromResultValue(IActionResult result)
        {
            var okResult = result as OkObjectResult;
            var claim = okResult?.Value as Claim;
            return claim;
        }
    }
}
