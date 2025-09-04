using AntiFraudAPI.DTO.Request;
using AntiFraudAPI.DTO.Response;
using AutoMapper;
using Domain;
using Interfaces.Managers;
using Microsoft.AspNetCore.Mvc;

namespace AntiFraudAPI.Controllers
{
    [Route("api/operations")]
    [ApiController]
    public class OperationController : Controller
    {
        private readonly IOperationManager _operationManager;
        private readonly IMapper _mapper;

        public OperationController(IOperationManager operationManager, IMapper mapper)
        {
            _operationManager = operationManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OperationResponse>> Create([FromBody] OperationRequest operationRequest)
        {
            var operation = _mapper.Map<OperationRequest, Operation>(operationRequest);

            var newOperation = await _operationManager.CreateOperation(operation);

            return Ok(_mapper.Map<OperationResponse>(newOperation));
        }

    }
}
