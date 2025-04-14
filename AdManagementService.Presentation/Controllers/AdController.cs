using AdManagementService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdManagementService.Presentation.Controllers
{
    [ApiController]
    [Route("api/ads")]
    public class AdController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAd([FromBody] CreateAdCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
