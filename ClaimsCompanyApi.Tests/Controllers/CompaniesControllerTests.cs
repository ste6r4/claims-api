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
            var companyResult = GetCompanyFromResultValue(result);

            result.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            companyResult?.Should().BeEquivalentTo(expectedResult);
            companyResult?.HasActivePolicy.Should().Be(expectedResult.HasActivePolicy);
        }
        
        [Fact]
        public async Task GetCompanyById_WithInvalidId_ReturnsNotFoundResult()
        {
            var companyId = 99;
            var query = new GetCompanyByIdQuery(companyId);
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync((Company)null!);
            
            var result = await _controller.GetCompanyById(companyId);
            
            result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetClaimsByCompany_WithValidCompanyId_ReturnsOkResult()
        {
            var companyId = FakeData.CompanyTwo!.Id;
            var query = new GetClaimsByCompanyQuery(companyId);
            var expectedResult = new List<Claim> { FakeData.ClaimOne! };
            _senderMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
            
            var result = await _controller.GetClaimsByCompany(companyId);
            var companyResult = GetCompanyFromResultValue(result);

            result.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            companyResult?.Claims.Should().BeEquivalentTo(expectedResult);
            companyResult?.Claims.Should().HaveCount(1);
            companyResult?.Claims.Any(x => x.UCR == FakeData.ClaimOne?.UCR).Should().BeFalse();
            companyResult?.Claims.Any(x=>x.UCR == FakeData.ClaimTwo?.UCR).Should().BeTrue();
        }

        private static Company? GetCompanyFromResultValue(IActionResult result)
        {
            var okResult = result as OkObjectResult;
            var companyResult = okResult?.Value as Company;
            return companyResult;
        }
    }
}
