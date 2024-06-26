using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Handlers.Queries;
using FluentAssertions;

namespace ClaimsCompanyApi.Tests.Handlers;

[Collection(TestRunSetup)]
public class CompanyHandlersTests : TestBase
{
    [Fact]
    public async Task Handle_GivenValidCompanyId_ReturnsCompany()
    {
        var handler = new GetCompanyByIdQueryHandler(AppDbContext);
   
        var result = await handler.Handle(new GetCompanyByIdQuery(FakeData.CompanyOne!.Id), default);

        result.Should().NotBeNull();
        result?.Name.Should().Be(FakeData.CompanyOne.Name);
        result?.Address1.Should().Be(FakeData.CompanyOne.Address1);
        result?.Address2.Should().Be(FakeData.CompanyOne.Address2);
        result?.Address3.Should().Be(FakeData.CompanyOne.Address3);
        result?.Postcode.Should().Be(FakeData.CompanyOne.Postcode);
        result?.Country.Should().Be(FakeData.CompanyOne.Country); 
        result?.Active.Should().Be(FakeData.CompanyOne.Active);
        result?.InsuranceEndDate.Should().Be(FakeData.CompanyOne.InsuranceEndDate);
        result?.Claims.Should().BeEmpty();
    }
}