using ClaimsCompanyApi.Handlers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsCompanyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var result = await _mediator.Send(new GetCompanyByIdQuery(id));
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpGet("{companyId}/claims")]
        public async Task<IActionResult> GetClaimsByCompany(int companyId)
        {
            var result = await _mediator.Send(new GetClaimsByCompanyQuery(companyId));
            return Ok(result);
        }
    }
}
