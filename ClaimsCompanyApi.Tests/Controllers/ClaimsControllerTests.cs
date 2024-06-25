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
            var query = new GetClaimByIdQuery(FakeData.ClaimOne!.UCR);
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(FakeData.ClaimOne);
            
            var result = await _claimsController.GetClaimById(query.Ucr);
            
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be(FakeData.ClaimOne);
        }

        [Fact]
        public async Task GetClaimById_WithInvalidId_ReturnsNotFoundResult()
        {
            const string claimId = "99";
            var query = new GetClaimByIdQuery(claimId);
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync((Claim)null!);
            
            var result = await _claimsController.GetClaimById(claimId);
            
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateClaim_WithValidClaim_ReturnsOkResult()
        {
            var claim = FakeData.ClaimOne;
            var command = new UpdateClaimCommand(claim!);
            var expectedResult = new Claim
            {
                Id = claim!.Id,
                UCR = claim.UCR,
                CompanyId = claim.CompanyId,
                ClaimDate = claim.ClaimDate,
                LossDate = claim.LossDate,
                AssuredName = claim.AssuredName,
                IncurredLoss = 1050,
                Closed = claim.Closed
            };
            _senderMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            
            var result = await _claimsController.UpdateClaim(claim);
            
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be(expectedResult);
        }

        [Fact]
        public async Task UpdateClaim_WithInvalidClaim_ReturnsNotFoundResult()
        {
            var claim = new Claim { Id = 99, UCR = "Error"};
            var command = new UpdateClaimCommand(claim);
            _senderMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync((Claim)null!);
            
            var result = await _claimsController.UpdateClaim(claim);
            
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
