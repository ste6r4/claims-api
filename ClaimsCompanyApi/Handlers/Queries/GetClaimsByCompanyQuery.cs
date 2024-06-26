using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Handlers.Queries;

public record GetClaimsByCompanyQuery(int CompanyId) : IRequest<List<Claim>>;

public class GetClaimsByCompanyQueryHandler : IRequestHandler<GetClaimsByCompanyQuery, List<Claim>>, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _context;

    public GetClaimsByCompanyQueryHandler(AppDbContext context)
    {
        _context = FakeContextHelper.CreateInMemoryDbContext();
    }

    public async Task<List<Claim>> Handle(GetClaimsByCompanyQuery request, CancellationToken cancellationToken)
    {
        var claims = await _context.Claims
            .Where(c => c.CompanyId == request.CompanyId)
            .Select(x => Claim.ClaimWithCompanyAttached(x))
            .ToListAsync(cancellationToken);

        return claims;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}