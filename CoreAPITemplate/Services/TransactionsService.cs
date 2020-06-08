using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using CoreAPI.Models;
using CoreAPI.Helpers;

namespace CoreAPI.Services
{
    public interface ITransactionsService
    {
        Task<IEnumerable<Transaction>> GetAll();
        Task<Transaction> GetOneByGuid(Guid guid);
        Task<IEnumerable<Transaction>> GetAllByIban(String Iban);
        Task<Transaction> AddOne(Transaction transaction);
        Task<Transaction> DeleteOneByGuid(Guid guid);
        Task<Transaction> UpdateOne(Transaction transaction);
        Task<Transaction> AddMany(IEnumerable<Transaction> transactions);
    }

    public class TransactionsService : ITransactionsService, IDisposable
    {

        private TransactionDBContext _transactionDBContext;
        private readonly AppSettings _appSettings;
        private readonly ILogger<TransactionsService> _logger;

        public TransactionsService(ILogger<TransactionsService> logger, IOptions<AppSettings> appSettings, TransactionDBContext context)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _transactionDBContext = context;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _transactionDBContext.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetOneByGuid(Guid guid)
        {

            return await _transactionDBContext.Transactions.FirstOrDefaultAsync(a => a.Id == guid);
        }
        public async Task<IEnumerable<Transaction>> GetAllByIban(String Iban)
        {
            return await _transactionDBContext.Transactions.Where(t => t.AccountIban == Iban).ToListAsync();
        }

        public async Task<Transaction> AddOne(Transaction transaction)
        {
            if (transaction.Id != null)
            {
                Transaction transaction_toadd = await GetOneByGuid(transaction.Id);
                if (transaction_toadd != null)
                {
                    return null;
                }
            }
            transaction.Id = Guid.NewGuid();
            if (TransactionValidation(transaction))
            {
                _transactionDBContext.Transactions.Add(transaction);
                if (await _transactionDBContext.SaveChangesAsync() > 0)
                {
                    return transaction;
                }
            }
            return null;
        }

        public async Task<Transaction> AddMany(IEnumerable<Transaction> transactions)
        {
            Transaction lastransaction = null; 
            foreach (Transaction transaction in transactions)
            {
                lastransaction = transaction;
                if (transaction.Id != null)
                {
                    Transaction transaction_toadd = await GetOneByGuid(transaction.Id);
                    if (transaction_toadd != null)
                    {
                        return null;
                    }
                }
                transaction.Id = Guid.NewGuid();
                if (TransactionValidation(transaction))
                {
                    _transactionDBContext.Transactions.Add(transaction);
 
                }
            }

            if (await _transactionDBContext.SaveChangesAsync() > 0)
            {
                return lastransaction;
            }

            return null;
        }

 



        public async Task<Transaction> UpdateOne(Transaction transaction)
        {
            Transaction transaction_toupdate = await GetOneByGuid(transaction.Id);
            if (transaction_toupdate != null)
            {
                transaction_toupdate.Amount = transaction.Amount;
            }
            if (await _transactionDBContext.SaveChangesAsync() > 0)
            {
                return transaction_toupdate;
            }
            return null;
        }

        public async Task<Transaction> DeleteOneByGuid(Guid guid)
        {
            Transaction transaction_todelete = await GetOneByGuid(guid);
            if (transaction_todelete != null)
            {
                _transactionDBContext.Remove(transaction_todelete);
                if (await _transactionDBContext.SaveChangesAsync() > 0)
                {
                    return transaction_todelete;
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
        // ~TransactionsService()
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




        private static bool TransactionValidation(Transaction transaction)
        {
            ValidationContext vc = new ValidationContext(transaction);
            ICollection<ValidationResult> results = new List<ValidationResult>(); // Will contain the results of the validation
            bool isValid = Validator.TryValidateObject(transaction, vc, results, true); // Validates the object and its properties using the previously created context.
            return isValid;
        }


    }
}
