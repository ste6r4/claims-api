using ClaimsCompanyApi.Data;
using ClaimsCompanyApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Handlers.Commands
{
    public record UpdateClaimCommand(Claim UpdatedClaim) : IRequest<Claim?>;

    public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, Claim?>, IDisposable, IAsyncDisposable
    {
        private readonly AppDbContext? _context;
        
        public UpdateClaimCommandHandler(AppDbContext? context)
        {
            _context =  FakeContextHelper.CreateInMemoryDbContext();
        }

        public async Task<Claim?> Handle(UpdateClaimCommand request, CancellationToken cancellationToken)
        {
            var existingClaim = await _context?.Claims
                .FirstOrDefaultAsync(c => c.UCR.ToLower() == request.UpdatedClaim.UCR.ToLower(), cancellationToken)!;
            if (existingClaim is null) return null;
            existingClaim.ClaimDate = request.UpdatedClaim.ClaimDate;
            existingClaim.LossDate = request.UpdatedClaim.LossDate;
            existingClaim.AssuredName = request.UpdatedClaim.AssuredName;
            existingClaim.IncurredLoss = request.UpdatedClaim.IncurredLoss;
            existingClaim.Closed = request.UpdatedClaim.Closed;
            _context.Claims.Update(existingClaim);
            await _context.SaveChangesAsync(cancellationToken);
            return Claim.ClaimWithCompanyAttached(existingClaim);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context!.DisposeAsync();
        }
    }
}
