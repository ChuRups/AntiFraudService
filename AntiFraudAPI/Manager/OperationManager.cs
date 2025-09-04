using Domain;
using Domain.Enums;
using Interfaces.Managers;
using Interfaces.Repositories;
using Interfaces.Validators;
using Manager.Producer;
using Microsoft.Extensions.Options;

namespace Manager
{
    public class OperationManager : IOperationManager
    {
        private readonly IOperationRepository _operationRepository;
        private readonly ICustomValidator<Operation> _operationValidator;
        private readonly IProducer _producer;
        private readonly IOptions<KafkaSettings> _kafkaSettings;

        public OperationManager(
            IOperationRepository operationRepository,
            ICustomValidator<Operation> operationValidator,
            IProducer producer,
            IOptions<KafkaSettings> kafkaSettings)
        {
            _operationRepository = operationRepository;
            _operationValidator = operationValidator;
            _producer = producer;
            _kafkaSettings = kafkaSettings;
        }

        public async Task<Operation> CreateOperation(Operation operation)
        {
            operation.OperationDate = DateTime.UtcNow;
            operation.Id = Guid.NewGuid();
            if (!await _operationValidator.IsValidAsync(operation, ActionType.Create))
            {
                throw new Exception(String.Join(",", _operationValidator.Errors));
            }

            await _operationRepository.InsertAsync(operation);

            try
            {
                var @event = new OperationEvent(operation.Id, operation.OperationDate);
                await _producer.SendAsync(_kafkaSettings.Value.Topic, @event);
            }
            catch (Exception ex)
            {
                //Nothing                
            }

            return operation;
        }
    }
}
