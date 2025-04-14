using AdDeliveryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdDeliveryService.Presentation.Controllers
{
    [ApiController]
    [Route("api/ads")]
    public class AdDeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdDeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAds()
        {
            var ads = await _mediator.Send(new GetActiveAdsQuery());
            return Ok(ads);
        }
    }
}
