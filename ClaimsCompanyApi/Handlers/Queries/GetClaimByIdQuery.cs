using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Handlers.Queries;

public record GetClaimByIdQuery(string Ucr) : IRequest<Claim?>;

public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, Claim?>, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _context ;

    public GetClaimByIdQueryHandler(AppDbContext context)
    {
        _context = FakeContextHelper.CreateInMemoryDbContext();
    }

    public async Task<Claim?> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
    {
        var claim = await _context.Claims
            .Where(c => c.UCR == request.Ucr)
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