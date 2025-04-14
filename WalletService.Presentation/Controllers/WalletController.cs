using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands;
using WalletService.Application.Queries;
using WalletService.Core.Domain.Entities;

namespace WalletService.Presentation.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("deduct")]
        public async Task<IActionResult> Deduct([FromBody] DeductWalletCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance([FromQuery] Guid userId, [FromQuery] AccountType accountType)
        {
            var balance = await _mediator.Send(new GetWalletBalanceQuery
            {
                UserId = userId,
                AccountType = accountType
            });
            return Ok(balance);
        }
    }
}
