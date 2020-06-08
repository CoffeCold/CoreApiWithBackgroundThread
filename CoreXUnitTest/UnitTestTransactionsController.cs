using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Xunit;

using CoreAPI.Controllers;
using CoreAPI.Services;
using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreXUnitTest
{
    public class UnitTestTransactionsController
    {
        private ILogger<TransactionsController> _transactionsControllerLogger;
        private ILogger<TransactionsService> _transactionsServiceLogger;
        private ITransactionsService _transactionsService;
        private IOptions<AppSettings> _appSettings;
        private TransactionDBContext _transactionDBContext;

        [Fact(DisplayName = "TransactionsController POST: api/Transactions returns newly created transaction ")]
        public async Task Create_ReturnsNewlyCreatedTransaction()
        {
            // Arrange
            GetInjections();
            var transactioncontroller = new TransactionsController(_transactionsControllerLogger, _transactionsService);

            // Act
            var actionresult = await transactioncontroller.PostTransaction(new Transaction() { AccountIban = "12AS12432546789", Amount = (Decimal)12.33, Counterparty = "24EZ984364392445", Date = DateTime.Now });


            // Assert
            OkObjectResult oresult = Assert.IsType<OkObjectResult>(actionresult.Result);
            Transaction trans = Assert.IsType<Transaction>(oresult.Value);
            Assert.Equal("12AS12432546789", trans.AccountIban);
        }

        [Fact(DisplayName = "TransactionsController POST: api/Transactions bulk insert transactions ")]
        public async Task Create_ReturnsNewlyCreatedTransactions()
        {
            // Arrange
            GetInjections();
            var transactioncontroller = new TransactionsController(_transactionsControllerLogger, _transactionsService);

            // Act
            List<Transaction> transactions = new List<Transaction>();

            transactions.Add(new Transaction() { AccountIban = "12AS12432546789", Amount = (Decimal)12.33, Counterparty = "24EZ984364392445", Date = DateTime.Now });
            transactions.Add(new Transaction() { AccountIban = "12AS12432546789", Amount = (Decimal)13.33, Counterparty = "24EZ984364396445", Date = DateTime.Now });
            transactions.Add(new Transaction() { AccountIban = "12AS12432546789", Amount = (Decimal)14.33, Counterparty = "24EZ984364397445", Date = DateTime.Now });
            transactions.Add(new Transaction() { AccountIban = "12AS12432546789", Amount = (Decimal)15.33, Counterparty = "24EZ984364398445", Date = DateTime.Now });
            var actionresult = await transactioncontroller.PostTransactions(transactions);


            // Assert
            OkObjectResult oresult = Assert.IsType<OkObjectResult>(actionresult.Result);
            Transaction trans = Assert.IsType<Transaction>(oresult.Value);
            Assert.Equal("12AS12432546789", trans.AccountIban);
        }


        #region injections
        private void GetInjections()
        {
            _transactionsControllerLogger = (_transactionsControllerLogger is null) ? GetTransactionsControllerLogger() : _transactionsControllerLogger;
            _transactionsServiceLogger = (_transactionsServiceLogger is null) ? GetTransactionsServiceLogger() : _transactionsServiceLogger;
            _transactionDBContext = (_transactionDBContext is null) ? GetContext() : _transactionDBContext;
            _appSettings = (_appSettings is null) ? GetAppSettings() : _appSettings;
            _transactionsService = (_transactionsService is null) ? GetTransactionService(_transactionsServiceLogger, _appSettings, _transactionDBContext) : _transactionsService;
        }

        private ILogger<TransactionsService> GetTransactionsServiceLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<TransactionsService> logger = new Logger<TransactionsService>(loggerFactory);
            return logger;
        }

        private TransactionDBContext GetContext()
        {
            var options = new DbContextOptionsBuilder<TransactionDBContext>()
             .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TransactionDB;ConnectRetryCount=0").Options;
            var context = new TransactionDBContext(options);
            return context;
        }
        private ILogger<TransactionsController> GetTransactionsControllerLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<TransactionsController> logger = new Logger<TransactionsController>(loggerFactory);
            return logger;
        }
        private ITransactionsService GetTransactionService(ILogger<TransactionsService> logger, IOptions<AppSettings> appsettings, TransactionDBContext context)
        {
            return new TransactionsService(logger, appsettings, context);
        }

        private static IOptions<AppSettings> GetAppSettings()
        {
            return Options.Create<AppSettings>(new AppSettings() { Foo = "", Bar = "" });
        }

        #endregion

    }
}
