using EMS.APPLICATION.Dtos;
using EMS.APPLICATION.Features.Transaction.Commands;
using EMS.CORE.Entities;
using EMS.CORE.Enums;
using EMS.CORE.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMS.TESTS.FeaturesTests.TransactionTests.CommandsTests
{

    [TestClass]
    public class AddTransactionCommandHandlerTests
    {
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private Mock<UserManager<AppUserEntity>> _mockUserManager;
        private Mock<ILogsRepository> _mockLogsRepository;
        private AddTransactionCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockLogsRepository = new Mock<ILogsRepository>();

            var store = new Mock<IUserStore<AppUserEntity>>();

            _mockUserManager = new Mock<UserManager<AppUserEntity>>(store.Object, null, null, null, null, null, null, null, null);

            _handler = new AddTransactionCommandHandler(_mockTransactionRepository.Object, _mockUserManager.Object, _mockLogsRepository.Object);
        }

        [TestMethod]
        public async Task Handle_AddTransaction_Returns_TransactionGetDto()
        {
            // Arrange
            var user = new AppUserEntity
            {
                Id = "user-id-123",
                UserName = "test-user"
            };

            _mockUserManager
                .Setup(x => x.FindByNameAsync(user.UserName))
                .ReturnsAsync(user);

            var transactionDto = new TransactionCreateDto
            {
                Name = "Transaction",
                Category = CategoryType.Expense,
                Amount = 250.75m
            };

            var budgetId = Guid.NewGuid();

            TransactionEntity savedTransaction = null;

            _mockTransactionRepository.Setup(x => x.AddTransactionAsync(It.IsAny<TransactionEntity>())).Callback<TransactionEntity>(x => savedTransaction = x).ReturnsAsync((TransactionEntity x) => x);

            var command = new AddTransactionCommand(transactionDto, budgetId, user.UserName);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(transactionDto.Name, result.Value.Name);
            Assert.AreEqual(transactionDto.Category, result.Value.Category);
            Assert.AreEqual(transactionDto.Amount, result.Value.Amount);
            Assert.IsNotNull(savedTransaction);
            Assert.AreEqual(budgetId, savedTransaction.BudgetId);
            Assert.AreEqual(transactionDto.Name, savedTransaction.Name);
            Assert.AreEqual(transactionDto.Amount, savedTransaction.Amount);
            _mockTransactionRepository.Verify(x => x.AddTransactionAsync(It.IsAny<TransactionEntity>()), Times.Once);
            _mockUserManager.Verify(x => x.FindByNameAsync(user.UserName), Times.Once);
        }
    }
}