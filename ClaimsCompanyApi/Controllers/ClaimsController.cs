﻿using ClaimsCompanyApi.Handlers.Commands;
using ClaimsCompanyApi.Handlers.Queries;
using ClaimsCompanyApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsCompanyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClaimsController(IMediator mediator)
        {
            _mediator = mediator;
        }
         
        [HttpGet("{ucr}")]
        public async Task<IActionResult> GetClaimById(string ucr)
        {
            var result = await _mediator.Send(new GetClaimByUcrQuery(ucr));
            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClaim(Claim updatedClaim)
        {
            var result = await _mediator.Send(new UpdateClaimCommand(updatedClaim));
            return result is not null ? Ok(result) : NotFound();
        }
    }
}
