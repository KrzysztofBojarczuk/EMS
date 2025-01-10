using AutoMapper;
using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Extensions;
using EMS.APPLICATION.Features.Employee.Commands;
using EMS.APPLICATION.Features.Transaction.Commands;
using EMS.APPLICATION.Features.Transaction.Queries;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ISender sender, UserManager<AppUserEntity> userManager, IMapper mapper) : ControllerBase
    {
        [HttpPost("{budgetId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddTransactionToUserAsync([FromRoute] Guid budgetId, [FromBody] TransactionCreateDto transactioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transactionEntity = mapper.Map<TransactionEntity>(transactioDto);

            transactionEntity.BudgetId = budgetId;

            var result = await sender.Send(new AddTransactionCommand(transactionEntity));

            return Ok(result);
        }

        [HttpGet("{budgetId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetTransactionsByBudgetId([FromRoute] Guid budgetId, [FromQuery] List<CategoryType> category, string? searchTerm = null)
        {
            var result = await sender.Send(new GetTransactionByBudgetIdQuery(budgetId, category, searchTerm));

            var budgetDtos = mapper.Map<IEnumerable<TransactionGetDto>>(result);

            return Ok(result);
        }
    }
}
