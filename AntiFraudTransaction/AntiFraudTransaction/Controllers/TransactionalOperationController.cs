using AutoMapper;
using Domain;
using Infrastructure.DTO;
using Interfaces.Manager;
using Microsoft.AspNetCore.Mvc;

namespace AntiFraudTransaction.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionalOperationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITransactionalOperationManager _operationManager;

        public TransactionalOperationController(ITransactionalOperationManager operationManager, IMapper mapper)
        {
            _operationManager = operationManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OperationResponse>> Create([FromBody] OperationRequest operationRequest)
        {
            var operation = _mapper.Map<OperationRequest, TransactionalOperation>(operationRequest);

            var newOperation = await _operationManager.CreateTransactionalOperation(operation);

            return Ok(_mapper.Map<OperationResponse>(newOperation));
        }
    }
}
