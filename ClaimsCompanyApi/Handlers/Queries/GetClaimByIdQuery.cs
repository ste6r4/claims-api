using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Handlers.Queries;

public record GetClaimByUcrQuery(string Ucr) : IRequest<Claim?>;

public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByUcrQuery, Claim?>, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _context ;

    public GetClaimByIdQueryHandler(AppDbContext context)
    {
        _context = FakeContextHelper.CreateInMemoryDbContext();
    }

    public async Task<Claim?> Handle(GetClaimByUcrQuery request, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .Where(c => c.UCR.ToLower() == request.Ucr.ToLower())
            .Select(x => Claim.ClaimWithCompanyAttached(x))
            .FirstOrDefaultAsync(cancellationToken);
        return claim;
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