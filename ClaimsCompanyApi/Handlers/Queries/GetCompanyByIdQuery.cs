using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Handlers.Queries;

public record GetCompanyByIdQuery(int Id) : IRequest<Company?>;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Company?>, IDisposable, IAsyncDisposable
{
    private readonly AppDbContext _context;

    public GetCompanyByIdQueryHandler(AppDbContext context)
    {
        _context = FakeContextHelper.CreateInMemoryDbContext();
    }

    public async Task<Company?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        return company;
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