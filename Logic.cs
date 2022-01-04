using Accounts;
using Number;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public interface IAccountLogic
    {
        /// <summary>
        /// Withdraws a given amount from a given accountNumber
        /// </summary>
        /// <param name="accountNumber">Indetifier of the account we want to withdraw money from</param>
        /// <param name="amount">The amount we want to withdraw from the account</param>
        /// <returns>The balance of the account after withdrawing the amount, or -1 if unable to make a withdraw</returns>
        decimal WithdrawAmount(int accountNumber, decimal amount);

        decimal DepositAmount(int accountNumber, decimal amount);

    }

    public class AccountLogic : IAccountLogic
    {
        IAccountData _accountAccessor;
        public AccountLogic(IAccountData accountAccessor)
        {
            _accountAccessor = accountAccessor;
        }

        public decimal DepositAmount(int accountNumber, decimal amount)
        {
            // Get the account
            Account account = _accountAccessor.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine($"UNABLE TO DEPOSIT {amount} FROM ACCOUNT {accountNumber} - ACCOUNT DOES NOT EXISTS.");
                return -1;
            }

            // Verify the amount you are depositing 
            if (account.Amount > amount)
            {
                Console.WriteLine($"YOU ARE DEPOSITING {amount} TO THIS ACCOUNT {accountNumber}.");
                return -1;
            }
            // Modify the balance of the account
            account.Amount -= account.Amount;

            // Update the account
            _accountAccessor.UpdateAccount(account);

            // Return the updated balance of the account
            return account.Amount;
        }

        public decimal WithdrawAmount(int accountNumber, decimal amount)
        {
            // Get the account
            Account account = _accountAccessor.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine($"UNABLE TO WITHDRAW {amount} FROM ACCOUNT {accountNumber} - ACCOUNT DOES NOT EXIST.");
                return -1;
            }

            // Verify that we have enough balance on the account
            if (account.Amount < amount)
            {
                Console.WriteLine($"UNABLE TO WITHDRAW {amount} FROM ACCOUNT {accountNumber} - NOT ENOUGHT BALANCE, ACCOUNT BALANCE {account.Amount}.");
                return -1;
            }

            // Modify the balance of the account
            account.Amount = account.Amount - amount;

            // Update the account
            _accountAccessor.UpdateAccount(account);

            // Return the updated balance of the account
            return account.Amount;
        }
    }
}
