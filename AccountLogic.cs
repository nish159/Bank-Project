using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{

    public class AccountLogic : IAccountLogic
    {
        IAccountRepository _accountRepository;
        IUserRepository _userRepository;
        public AccountLogic(IAccountRepository accountRespository, IUserRepository userRepository)
        {
            _accountRepository = accountRespository;
            _userRepository = userRepository;
        }

        public decimal DepositAmount(int accountNumber, decimal amount)
        {
            // Get the account
            Account account = _accountRepository.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine($"Unable to deposit {amount} from account {accountNumber} - Account does not exist.");
                return -1;
            }

            // Verify the amount you are depositing 
            if (amount < 0)
            {
                Console.WriteLine($"You are depositing {amount} to this account {accountNumber}.");
                return -1;
            }
            
            // Modify the balance of the account
            account.Amount = account.Amount + amount;

            // Update the account
            _accountRepository.UpdateAccount(account);

            // Return the updated balance of the account
            return account.Amount;
        }

        public decimal DepositAmount(string firstName, string lastName, int accountNumber, decimal amount)
        {
            Validate(firstName, lastName, accountNumber);

            // Call deposit function
            return DepositAmount(accountNumber, amount);
        }

        public decimal TransferAmount(int sourceAccountNumber, string sourcePin, int destAccountNumber, string destFirstName, string destLastName, decimal amount)
        {
            // Validate the destination account information
            if (Validate(destFirstName, destLastName, destAccountNumber) == false)
            {
                Console.WriteLine($"Unable to transfer {amount} from {sourceAccountNumber} to {destAccountNumber}");
                return -1;
            }

            // A transfer is a combination of a withdraw and a deposit 
            decimal withdrawResult = WithdrawAmount(sourceAccountNumber, sourcePin, amount);
            if (withdrawResult == -1)
            {
                Console.WriteLine($"Unable to transfer {amount} from {sourceAccountNumber} to {destAccountNumber}");
                return -1;
            }

            decimal depositResult = DepositAmount(destFirstName, destLastName, destAccountNumber, amount);
            if (depositResult == -1)
            {
                Console.WriteLine($"Unable to transfer {amount} from {sourceAccountNumber} to {destAccountNumber}");
                return -1;
            }

            return withdrawResult;
        }

        public decimal WithdrawAmount(int accountNumber, decimal amount)
        {
            // Get the account
            Account account = _accountRepository.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine($"Unable to withdraw {amount} from account {accountNumber} - account does not exist.");
                return -1;
            }

            // Verify that we have enough balance on the account
            if (account.Amount < amount)
            {
                Console.WriteLine($"Unable to withdraw {amount} from account {accountNumber} - not enough balance, account balance {account.Amount}.");
                return -1;
            }

            // Modify the balance of the account
            account.Amount = account.Amount - amount;

            // Update the account
            _accountRepository.UpdateAccount(account);

            // Return the updated balance of the account
            return account.Amount;
        }

        public decimal WithdrawAmount(int accountNumber, string pin, decimal amount)
        {
            // Verify the account number
            Account account = _accountRepository.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine("This account does not exist.");
                return -1;
            }

            if (pin != account.Pin)
            {
                Console.WriteLine("Incorrect pin.");
                return -1;
            }
            return WithdrawAmount(accountNumber, amount);
        }

        private bool Validate(string firstName, string lastName, int accountNumber)
        {
            // Verify the account number 
            Account account = _accountRepository.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine("This account does not exist.");
                return false;
            }

            // Verify the first and last name
            User user = _userRepository.GetByUserName(account.UserName);

            if (firstName != user.FirstName || lastName != user.LastName)
            {
                Console.WriteLine("The names do not match the account number.");
                return false;
            }
            return true;
        }
    }
}
