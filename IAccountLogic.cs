namespace Logic
{
    using Bank;
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

        Task<Result<decimal>> DepositAmount(string firstName, string lastName, int accountNumber, decimal amount);
        
        Task<Result<decimal>> DepositAmount(int accountNumber, decimal amount);

        Task<Result<decimal>> TransferAmount(int sourceAccountNumber, string sourcePin, int destAccountNumber, string destFirstName, string destLastName, decimal amount);
    }
}
