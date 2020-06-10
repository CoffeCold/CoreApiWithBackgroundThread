using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreAPI.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAll();
        Task<Account> GetOneByIban(String Iban);
        Task<Account> GetOneByIbanWithTransactions(String Iban);
        Task<Account> GetOneByName(string name);
        Task<Account> AddOne(Account account);
        Task<Account> DeleteOneByIban(String Iban);
        Task<Account> UpdateOne(Account account);
    }

    public class AccountService : IAccountService, IDisposable
    {

        private readonly TransactionDBContext _transactionDBContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger, IOptions<AppSettings> appSettings, TransactionDBContext context)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _transactionDBContext = context;
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _transactionDBContext.Accounts.ToListAsync();
        }
        public async Task<Account> GetOneByIban(String Iban)
        {
            return await _transactionDBContext.Accounts.FirstOrDefaultAsync(a => a.Iban == Iban);
        }

        public async Task<Account> GetOneByIbanWithTransactions(String Iban)
        {
            Account account =  await _transactionDBContext.Accounts.FirstOrDefaultAsync(a => a.Iban == Iban);
            if (account != null)
            {
                account.Transactions = await _transactionDBContext.Transactions.Where(t => t.AccountIban == Iban).ToListAsync();
                account.Transactions.Select(t => { t.Account = null; return t; }).ToList();
            }
            return account;
        }

        public async Task<Account> GetOneByName(string name)
        {
            return await _transactionDBContext.Accounts.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<Account> AddOne(Account account)
        {

            if (AccountValidation(account))
            {
                Account account_toadd = await GetOneByIban(account.Iban);

                if (account_toadd == null)
                {
                    _transactionDBContext.Accounts.Add(account);
                    if (await _transactionDBContext.SaveChangesAsync() > 0)
                    {
                        return account;
                    }
                }
            }

            return null;
        }



        public async Task<Account> UpdateOne(Account account)
        {
            if (AccountValidation(account))
            {
                Account account_toupdate = await GetOneByIban(account.Iban);
                if (account_toupdate != null)
                {
                    account_toupdate.Name = account.Name;
                    account_toupdate.City = account.City;
                }
                if (await _transactionDBContext.SaveChangesAsync() > 0)
                {
                    return account_toupdate;
                }
            }
 
            return null;
        }

        public async Task<Account> DeleteOneByIban(String Iban)
        {
            Account account_todelete = await GetOneByIban(Iban);
            if (account_todelete != null)
            {
                _transactionDBContext.Remove(account_todelete);
                if (await _transactionDBContext.SaveChangesAsync() > 0)
                {
                    return account_todelete;
                }
            }
            return null;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AccountServiceService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


        private static bool AccountValidation(Account account)
        {
            ValidationContext vc = new ValidationContext(account);
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(account, vc, results, true); // Validates the object and its properties using the previously created context.
            return isValid;
        }




    }
}
