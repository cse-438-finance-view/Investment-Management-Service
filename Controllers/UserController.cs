using InvestmentManagementService.Features.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(CreateUserCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommandRequest request)
        {
            var result = await _mediator.Send(request);

            if (result.Succeeded)
        {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
