namespace Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Number;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    public interface IAccountData
    {
        // function to get a list of all accounts in the system
        List<Account> GetAllAccounts();

        // function to get a list of all user accounts in the system
        List<Account> GetAllUserAccounts(string userName);

        // function to get a single account
        /// <summary>
        /// Gets an account with the given account number
        /// </summary>
        /// <param name="accountNumber">Unique account identifier</param>
        /// <returns>The <see cref="Account"/> with the given account number, or null if no account exists with that number</returns>

        // function to get a single account
        Account GetByAccountNumber(int accountNumber);

        // function to get by guid
        Account GetById(string userName);

        // function to create an account 
        void CreateAccount(Account account);

        // function to update the account information 
        void UpdateAccount(Account updatedAccount);

        // function to delete an account 
        void DeleteAccount(Account deletedAccount);
    }

    public class AccountData : IAccountData
    {

        public void CreateAccount(Account account)
        {
            List<Account> accounts = GetAllAccounts();
            // Checking if there is any account that has the same account number
            // as the account we want to create
            if (accounts.Any(i => i.Number == account.Number))
            {
                Console.WriteLine($"Unable to create account. Account number {account.Number} already exists");
                return;
            }

            accounts.Add(account);
            using (StreamWriter writer = new StreamWriter("../../../accounts.json"))
            {
                string accountsJson = JsonSerializer.Serialize(accounts);
                writer.Write(accountsJson);
            }
        }

        public void DeleteAccount(Account deletedAccount)
        {
            List <Account> allAccounts = GetAllAccounts();

            //allAccounts.Remove(deletedAccount);
            allAccounts = allAccounts.Where(i => i.Number != deletedAccount.Number).ToList();
            using (StreamWriter writer = new StreamWriter("../../../accounts.json"))
            {
                string accountsJson = JsonSerializer.Serialize(allAccounts);
                writer.Write(accountsJson);
            }

            Console.WriteLine($"Account {deletedAccount.Number} has been deleted");
        }

        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            
            using (StreamReader reader = new StreamReader("../../../accounts.json"))
            {
                string accountsJson = reader.ReadToEnd();
                accounts = JsonSerializer.Deserialize<List<Account>>(accountsJson);
            }

            return accounts;
        }

        public List<Account> GetAllUserAccounts(string userName)
        {
            // Get all accounts in the system (json file)
            List<Account> accounts = GetAllAccounts();

            // Filter the list to only have accounts for the given user name
            List<Account> userAccounts = accounts.Where(i => i.UserName == userName).ToList();
            return userAccounts;
        }

        public Account GetByAccountNumber(int accountNumber)
        {
            List<Account> accounts = GetAllAccounts();

            // account will either be the account with the matching account number, or null
            // if no account has that number.
            Account account = accounts.Where(i => i.Number == accountNumber).FirstOrDefault();
            return account;
        }

        public Account GetById(string ID)
        {
            List<Account> accounts = GetAllAccounts();

            Account account = accounts.Where(i => i.Id == ID).FirstOrDefault();
            return account;
        }

        public void UpdateAccount(Account updatedAccount)
        {
            // Check if the account we want to update exists
            Account existingAccount = GetByAccountNumber(updatedAccount.Number);
            if (existingAccount == null)
            {
                Console.WriteLine($"Unable to update account. No account with account number {updatedAccount.Number} exists");
                // function executes until it meets first return statement 
                return;
            }

            // We have verified that the account exists - update the account
            List<Account> accounts = GetAllAccounts();
            foreach(Account account in accounts )
            {
                if (account.Number == updatedAccount.Number)
                {
                    account.Amount = updatedAccount.Amount;
                    account.UserName = updatedAccount.UserName;
                }
            }
            using (StreamWriter writer = new StreamWriter("../../../accounts.json"))
            {
                string accountsJson = JsonSerializer.Serialize(accounts);
                writer.Write(accountsJson);
            }
        }
    }
}

namespace Number
{
    using System;
    
    public class Account
    {
        // each one should be unique
        public string Id { get; set; }
        public string UserName { get; set; }
        public int Number { get; set; }
        public decimal Amount { get; set; }
        public string Pin { get; set; }
        public Account()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
