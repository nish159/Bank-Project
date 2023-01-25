namespace Logic
{
    using Bank;
    using OnlineBankingProject.Common.Models;
    using System.Threading.Tasks;

    public interface IAccountLogic
    {
        /// <summary>
        /// Withdraws a given amount from a given accountNumber
        /// </summary>
        /// <param name="accountNumber">Indetifier of the account we want to withdraw money from</param>
        /// <param name="amount">The amount we want to withdraw from the account</param>
        /// <returns>The balance of the account after withdrawing the amount, or -1 if unable to make a withdraw</returns>
        Task<Result<decimal>> WithdrawAmount(int accountNumber, decimal amount);

        Task<Result<decimal>> WithdrawAmount(int accountNumber, string pin, decimal amount);

        /// <summary>
        /// Withdraws the given amount from the given account
        /// </summary>
        /// <param name="accountId">Identifier of the account we want to withdraw money from</param>
        /// <param name="pin">The pin of the account the amount should be withdrawn from</param>
        /// <param name="amount">The amount to be withdrawn from the account</param>
        /// <returns>The account after the withdraw</returns>
        Task<Result<Account>> WithdrawAmount(string accountId, string pin, decimal amount);

        Task<Result<decimal>> DepositAmount(string firstName, string lastName, int accountNumber, decimal amount);
        
        Task<Result<decimal>> DepositAmount(int accountNumber, decimal amount);

        /// <summary>
        /// Withdraws the given amount from the given account
        /// </summary>
        /// <param name="accountId">Identifier of the account we want to deposit money into</param>
        /// <param name="amount">The amount to be deposited into the account</param>
        /// <param name="accountNumber">The account we want to deposit money into</param>
        /// <returns>The account after the deposit</returns>
        Task<Result<Account>> DepositAmount(string accountId, decimal amount, int accountNumber);

        Task<Result<decimal>> TransferAmount(int sourceAccountNumber, string sourcePin, int destAccountNumber, string destFirstName, string destLastName, decimal amount);
    }
}
