using FinancialSystem.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinancialSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountingService accountingService;

        public AccountController(IAccountingService accountingService)
        {
            this.accountingService = accountingService;
        }

        /// <summary>
        /// Gets current account balance.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Successfully got account balace.</response>
        [HttpGet("balance")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBalance()
        {
            var balance = await accountingService.GetCurrentBalance();
            return Ok(balance);
        }
    }
}
