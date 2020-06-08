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
    public class ApiAccountsControllerTests
    {
        private ILogger<AccountsController> _accountsControllerLogger;
        private ILogger<AccountService> _accountsServiceLogger;
        private IAccountService _accountsService;
        private IOptions<AppSettings> _appSettings;
        private TransactionDBContext _transactionDBContext;

        [Fact(DisplayName = "AccountsController POST: api/Accounts returns newly created account ")]
        public async Task Create_ReturnsNewlyCreatedAccount()
        {
            // Arrange
            GetInjections();
            var acccontroller = new AccountsController(_accountsControllerLogger, _accountsService);

            // Act
            var actionresult = await acccontroller.PostAccount(new Account() { Iban = "12xx993456", City = "Almere", Name = "Ouzu", Transactions = null });


            // Assert
            OkObjectResult oresult = Assert.IsType<OkObjectResult>(actionresult.Result);
            Account account = Assert.IsType<Account>(oresult.Value);
            Assert.Equal("12xx993456", account.Iban);
            Assert.Equal("Ouzu", account.Name);
        }

        [Fact(DisplayName = "AccountsController POST: api/Accounts should give BadRequest at Invalid model")]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange 
            GetInjections();
            var acccontroller = new AccountsController(_accountsControllerLogger, _accountsService);

            acccontroller.ModelState.AddModelError("error", "some error");
            // Act
            var actionresult = await acccontroller.PostAccount(new Account() {  Iban="12xx121212", City ="Hoensbroek", Name = "Verhaar", Transactions = null});

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionresult.Result);
        }

        [Fact(DisplayName = "AccountsController POST: api/Accounts should give BadRequest at invalid iban ")]
        public async Task Create_ReturnsBadRequest_GivenInvalidIban()
        {
            // Arrange & Act
            GetInjections();
            var acccontroller = new AccountsController(_accountsControllerLogger, _accountsService);

            // Act
            var actionresult = await acccontroller.PostAccount(new Account() { Iban = "12XXX121212", City = "Hoensbroek", Name = "Verhaar", Transactions = null });

            //Assert
            BadRequestResult oresult = Assert.IsType<BadRequestResult>(actionresult.Result);
        }


 
        #region injections
        private void GetInjections()
        {
            _accountsControllerLogger = (_accountsControllerLogger is null) ? GetAccountsControllerLogger() : _accountsControllerLogger;
            _accountsServiceLogger = (_accountsServiceLogger is null) ? GetAccountsServiceLogger() : _accountsServiceLogger;
            _transactionDBContext = (_transactionDBContext is null) ? GetContext() : _transactionDBContext;
            _appSettings = (_appSettings is null) ? GetAppSettings() : _appSettings;
            _accountsService = (_accountsService is null) ? GetAccountService(_accountsServiceLogger, _appSettings, _transactionDBContext) : _accountsService;
        }

        private ILogger<AccountService> GetAccountsServiceLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<AccountService> logger = new Logger<AccountService>(loggerFactory);
            return logger;
        }

        private TransactionDBContext GetContext()
        {
            var options = new DbContextOptionsBuilder<TransactionDBContext>()
             .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TransactionDB;ConnectRetryCount=0").Options;
            var context = new TransactionDBContext(options);
            return context;
        }
        private ILogger<AccountsController> GetAccountsControllerLogger()
        {
            ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();
            ILogger<AccountsController> logger = new Logger<AccountsController>(loggerFactory);
            return logger;
        }
        private IAccountService GetAccountService(ILogger<AccountService> logger, IOptions<AppSettings> appsettings, TransactionDBContext context)
        {
            return new AccountService(logger, appsettings, context);
        }

        private static IOptions<AppSettings> GetAppSettings()
        {
            return Options.Create<AppSettings>(new AppSettings() { Foo = "", Bar = "" });
        }

        #endregion

    }
}
