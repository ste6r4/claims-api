using ClaimsCompanyApi.Controllers;
using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Handlers.Queries;
using ClaimsCompanyApi.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClaimsCompanyApi.Tests.Controllers
{
    [Collection(TestRunSetup)]
    public class CompaniesControllerTests : TestBase
    {
        private readonly Mock<IMediator> _senderMock;
        private readonly CompaniesController _controller;

        public CompaniesControllerTests()
        {
            _senderMock = new Mock<IMediator>();
            _controller = new CompaniesController(_senderMock.Object);
        }

        [Fact]
        public async Task GetCompanyById_WithValidId_ReturnsOkResult()
        {
            var query = new GetCompanyByIdQuery(FakeData.CompanyOne!.Id);
            var expectedResult = FakeData.CompanyOne;
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            
            var result = await _controller.GetCompanyById(query.Id);
            
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be(expectedResult);
        }

        [Fact]
        public async Task GetCompanyById_WithInvalidId_ReturnsNotFoundResult()
        {
            var companyId = 99;
            var query = new GetCompanyByIdQuery(companyId);
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync((Company)null!);
            
            var result = await _controller.GetCompanyById(companyId);
            
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetClaimsByCompany_WithValidCompanyId_ReturnsOkResult()
        {
            var companyId = FakeData.CompanyOne!.Id;
            var query = new GetClaimsByCompanyQuery(companyId);
            var expectedResult = new List<Claim> { FakeData.ClaimOne! };
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            
            var result = await _controller.GetClaimsByCompany(companyId);
            
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().Be(expectedResult);
        }
    }
}
