using AutoMoqCore;
using Domain;
using Domain.Enums;
using Interfaces.Managers;
using Interfaces.Repositories;
using Interfaces.Validators;
using Manager.Producer;
using Microsoft.Extensions.Options;
using Moq;

namespace Manager.Test
{
    public class OperationManagerTest
    {
        private readonly AutoMoqer _autoMoqer;
        private readonly IOperationManager _operationManager;

        public OperationManagerTest()
        {
            _autoMoqer = new AutoMoqer();
            _operationManager = _autoMoqer.Resolve<OperationManager>();
        }

        [Fact]
        public async Task CreateOperation_WithoutTotalAmount()
        {
            // ARRANGE
            var operation = new Operation
            {
                IdCustomer = Guid.NewGuid(),
                Amount = 2000,
                OperationDate = DateTime.UtcNow
            };

            var mockValidator = _autoMoqer.GetMock<ICustomValidator<Operation>>();
            mockValidator.Setup(x => x.IsValidAsync(operation, ActionType.Create))
                .ReturnsAsync(true);

            var mockAbsenceRepository = _autoMoqer.GetMock<IOperationRepository>();
            mockAbsenceRepository.Setup(x => x.InsertAsync(It.IsAny<Operation>()))
                .Returns(Task.CompletedTask);

            SetUpKafkaOptions();

            // ACT
            var operationResponse = await _operationManager.CreateOperation(operation);

            // ASSERT
            mockAbsenceRepository.Verify(x => x.InsertAsync(It.IsAny<Operation>()), Times.Once);
        }

        [Fact]
        public async Task CreateOperation_AmountExceedsTransactionLimit_ShouldNotInsert()
        {
            // ARRANGE
            var operation = new Operation
            {
                IdCustomer = Guid.NewGuid(),
                Amount = 2500,
                OperationDate = DateTime.UtcNow
            };

            var listErrors = new List<string> { "HasExceededMaximumAmountPerTransferError" };

            var mockValidator = _autoMoqer.GetMock<ICustomValidator<Operation>>();
            mockValidator.Setup(x => x.IsValidAsync(operation, ActionType.Create))
                .ReturnsAsync(false);
            mockValidator.Setup(x => x.Errors)
                .Returns(listErrors);

            var mockRepository = _autoMoqer.GetMock<IOperationRepository>();
            mockRepository.Setup(x => x.InsertAsync(It.IsAny<Operation>()))
                .Returns(Task.CompletedTask);

            // ACT
            var ex = await Assert.ThrowsAsync<Exception>(() => _operationManager.CreateOperation(operation));

            // ASSERT
            mockRepository.Verify(x => x.InsertAsync(It.IsAny<Operation>()), Times.Never);
            Assert.Contains("HasExceededMaximumAmountPerTransferError", ex.Message);
        }

        [Fact]
        public async Task CreateOperation_AmountExceedsDailyLimit_ShouldNotInsert()
        {
            // ARRANGE
            var operation = new Operation
            {
                IdCustomer = Guid.NewGuid(),
                Amount = 2000,
                OperationDate = DateTime.UtcNow
            };

            var dailyTotal = 19000;
            var listErrors = new List<string> { "HasExceededMaximumDailyAmountError" };

            var mockValidator = _autoMoqer.GetMock<ICustomValidator<Operation>>();
            mockValidator.Setup(x => x.IsValidAsync(operation, ActionType.Create))
                .ReturnsAsync(false);
            mockValidator.Setup(x => x.Errors)
                .Returns(listErrors);
            var mockRepository = _autoMoqer.GetMock<IOperationRepository>();
            mockRepository.Setup(x => x.GetOperationsSumAsync(It.IsAny<Operation>()))
                .ReturnsAsync(dailyTotal);

            mockRepository.Setup(x => x.InsertAsync(It.IsAny<Operation>()))
                .Returns(Task.CompletedTask);

            // ACT
            var ex = await Assert.ThrowsAsync<Exception>(() => _operationManager.CreateOperation(operation));

            // ASSERT
            mockRepository.Verify(x => x.InsertAsync(It.IsAny<Operation>()), Times.Never);
            Assert.Contains("HasExceededMaximumDailyAmountError", ex.Message);
        }


        #region [PrivateMethods]
        void SetUpKafkaOptions()
        {
            var kafkaSettings = new KafkaSettings
            {
                Hostname = "localhost",
                Port = "9092",
                Topic = "test-topic"
            };

            var mockKafkaOptions = _autoMoqer.GetMock<IOptions<KafkaSettings>>();
            mockKafkaOptions.Setup(x => x.Value).Returns(kafkaSettings);

            var mockProducer = _autoMoqer.GetMock<IProducer>();
            mockProducer.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<OperationEvent>()))
                .Returns(Task.CompletedTask);
        }

        #endregion
    }
}