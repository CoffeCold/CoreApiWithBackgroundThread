using System;
using CoreAPI.Models;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace CreateLocalDB
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TransactionDBContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                PopulateDB(context);                
                context.SaveChanges();

            }

            using (var context = new TransactionDBContext())
            {
                Console.WriteLine(context.Set<Account>().FirstOrDefault().Iban);
            }
        }

        private static void PopulateDB(TransactionDBContext context)
        {
            var account1 = context.Add(new Account() { Name = "Janssen", City = "Amsterdam", Iban = "12AS12432546789" }).Entity;
            var account2 = context.Add(new Account() { Name = "Pietersen", City = "Rotterdam", Iban = "24EZ984364392445" }).Entity;
            var account3 = context.Add(new Account() { Name = "Dijkstra", City = "Leeuwarden", Iban = "36OP4579812465" }).Entity;

            var transaction1 = context.Add(new Transaction() { Account = account1, Amount = (Decimal)23.12 ,  Counterparty = "12AS12432546789" , Date = DateTime.Now, Id = Guid.NewGuid()}).Entity;
            var transaction2 = context.Add(new Transaction() { Account = account2, Amount = (Decimal)56, Counterparty = "24EZ984364392445", Date = DateTime.Now, Id = Guid.NewGuid() }).Entity;
            var transaction3 = context.Add(new Transaction() { Account = account3, Amount = (Decimal)67.76, Counterparty = "24EZ984364392445", Date = DateTime.Now, Id = Guid.NewGuid() }).Entity;
            var transaction4 = context.Add(new Transaction() { Account = account1, Amount = (Decimal)289.89, Counterparty = "36OP4579812465", Date = DateTime.Now, Id = Guid.NewGuid() }).Entity;
        }
    }
}
