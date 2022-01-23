namespace DataAccess
{
    using System.Collections.Generic;
    using Models;
    using Bank;

    /// <summary>
    /// Defines the interface for interacting with the account data store.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Gets a list of all accounts in the system
        /// </summary>
        /// <returns>A list of all <see cref="Account"/>s in the system</returns>
        Result<List<Account>> GetAllAccounts();

        /// <summary>
        /// Gets a list of all accounts that are belong to the user with a given user name
        /// </summary>
        /// <param name="userName">Unique identifier of the user we want to retrieve accounts for</param>
        /// <returns>A list of all <see cref="Account"/>s that belong to the user</returns>
        Result<List<Account>> GetAllByUsername(string userName);

        // function to get a single account
        /// <summary>
        /// Gets an account with the given account number
        /// </summary>
        /// <param name="accountNumber">Unique account identifier</param>
        /// <returns>The <see cref="Account"/> with the given account number, or null if no account exists with that number</returns>
        Result<Account> GetByAccountNumber(int accountNumber);

        /// <summary>
        /// Gets the account with the given account id
        /// </summary>
        /// <param name="id">Unique account identifier</param>
        /// <returns>The <see cref="Account"/> with the given id, or null if no account exists with that id</returns>
        Result<Account> GetById(string userName);

        /// <summary>
        /// Creates a new account data entity
        /// </summary>
        /// <param name="account">The account to be created</param>
        Result<Account> CreateAccount(Account account);

        /// <summary>
        /// Updates the specified account data entity
        /// </summary>
        /// <param name="updatedAccount">The account to be updated</param>
        Result<Account> UpdateAccount(Account updatedAccount);

        /// <summary>
        /// Deletes the specified account data entity
        /// </summary>
        /// <param name="deletedAccount">The account to be deleted</param>
        Result<Account> DeleteAccount(Account deletedAccount);
    }
}
