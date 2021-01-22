using FinancialSystem.Domain.Exceptions;
using FinancialSystem.Domain.Models;
using FinancialSystem.Domain.Services;
using FinancialSystem.Web.Models;
using FinancialSystem.Web.Models.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IValidator<TransactionRequest> transactionRequestValidator;
        private readonly IAccountingService accountingService;

        public TransactionsController(IValidator<TransactionRequest> transactionRequestValidator,
            IAccountingService accountingService)
        {
            this.transactionRequestValidator = transactionRequestValidator;
            this.accountingService = accountingService;
        }

        /// <summary>
        /// Get transactions history list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Transaction[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var transactions = await accountingService.GetTransactionsHistory();
            return Ok(transactions);
        }

        /// <summary>
        /// Gets transaction by id.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Successfully got transaction.</response>
		/// <response code="404">Transaction with such <paramref name="id"/> was not found.</response>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Transaction[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var transaction = await accountingService.GetTransactionById(id);

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        /// <summary>
        /// Creates new transaction.
        /// </summary>
        /// <param name="request">Transaction model.</param>
        /// <response code="201">Successfully added new transaction.</response>
		/// <response code="400">Bad request data.</response>
		/// <response code="409">Conflict occured during processing transaction.</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Transaction), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<ValidationError>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody] TransactionRequest request)
        {
            var validationResult = transactionRequestValidator.Validate(request);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => new ValidationError 
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage 
                }));

            try
            {
                var transaction = await accountingService.ProcessNewTransaction(request.Type, request.Amount);
                return Created($"{Request.Path}/{transaction.Id}", transaction);
            }
            catch (DebitException ex)
            {
                var errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        PropertyName = nameof(request.Amount),
                        ErrorMessage = ex.Message
                    }
                };

                return Conflict(errors);
            }
        }
    }
}
