using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Handlers.Commands;
using ClaimsCompanyApi.Models;
using FluentAssertions;

namespace ClaimsCompanyApi.Tests.Handlers
{
    [Collection(TestRunSetup)]
    public class UpdateClaimCommandHandlerTests : TestBase
    {
        [Fact(Skip = "DI pipeline issues")]
        public async Task Handle_ExistingClaim_ReturnsUpdatedClaim()
        {
            const int claimId = 10;
            var existingClaim = new Claim
            {
                Id = claimId,
                CompanyId = FakeData.CompanyOne!.Id,
                UCR = "UCR123",
                ClaimDate = DateTime.Now.AddDays(-5),
                LossDate = DateTime.Now.AddDays(-10),
                AssuredName = "Jane Doe",
                IncurredLoss = 500,
                Closed = false
            };
            var updatedClaim = new Claim
            {
                Id = claimId,
                CompanyId = FakeData.CompanyOne.Id,
                UCR = "UCR123",
                ClaimDate = DateTime.Now.AddDays(-5),
                LossDate = DateTime.Now.AddDays(-11),
                AssuredName = "John Doe",
                IncurredLoss = 1000,
                Closed = true
            };

            AppDbContext.Claims.Add(existingClaim);
            await AppDbContext.SaveChangesAsync();
            var command = new UpdateClaimCommand(updatedClaim);
            var handler = new UpdateClaimCommandHandler(AppDbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result!.Id.Should().Be(claimId);
            result.UCR.Should().Be(updatedClaim.UCR);
            result.ClaimDate.Should().Be(updatedClaim.ClaimDate);
            result.LossDate.Should().Be(updatedClaim.LossDate);
            result.AssuredName.Should().Be(updatedClaim.AssuredName);
            result.IncurredLoss.Should().Be(updatedClaim.IncurredLoss);
            result.Closed.Should().Be(updatedClaim.Closed);
        }

        [Fact]
        public async Task Handle_NonExistingClaim_ReturnsNull()
        {
            var updatedClaim = new Claim
            {
                UCR = "UCR123",
                ClaimDate = new DateTime(2022, 1, 1),
                LossDate = new DateTime(2022, 1, 2),
                AssuredName = "John Doe",
                IncurredLoss = 1000,
                Closed = true
            };

            var command = new UpdateClaimCommand(updatedClaim);
            var handler = new UpdateClaimCommandHandler(AppDbContext);
            
            var result = await handler.Handle(command, CancellationToken.None);
            
            result.Should().BeNull();
        }
    }
}
