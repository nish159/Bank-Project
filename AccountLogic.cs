namespace Logic
{
    using Bank;
    using DataAccess;
    using Models;
    using System;
    using System.Threading.Tasks;
    public class AccountLogic : IAccountLogic
    {
        IAccountRepository _accountRepository;
        IUserRepository _userRepository;

        public AccountLogic(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<decimal>> DepositAmount(int accountNumber, decimal amount)
        {
            // Get the account
            Result<Account> getByAccountNumberResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (getByAccountNumberResult.Succeeded == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = getByAccountNumberResult.ResultType,
                    Message = $"Unable to deposit {amount} to account {accountNumber}. Reason: {getByAccountNumberResult.Message}."
                };
            }
            Account account = getByAccountNumberResult.Value;

            // Verify the amount you are depositing 
            if (amount < 0)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.InvalidData,
                    Message = $"Unable to deposit {amount} from account {accountNumber} - Invalid amount."
                };
            }

            // Modify the balance of the account
            account.Amount = account.Amount + amount;

            // Update the account
            await _accountRepository.UpdateAccountAsync(account);

            // Return the updated balance of the account
            return new Result<decimal>()
            {
                Succeeded = true,
                Value = account.Amount
            };
        }

        public async Task<Result<decimal>> DepositAmount(string firstName, string lastName, int accountNumber, decimal amount)
        {
            Result<Account> getByAccountNumberResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (getByAccountNumberResult.Succeeded == false)
            {
                // Return Result<decimal>
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = getByAccountNumberResult.ResultType,
                    Message = $"Unable to deposit {amount} to account {accountNumber}. Reason: {getByAccountNumberResult.Message}."
                };
            }
            Account account = getByAccountNumberResult.Value;

            Result<BankUser> user = await _userRepository.GetByUserNameAsync(account.UserName);
            if (firstName != user.Value.FirstName || lastName != user.Value.LastName)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.NotFound,
                    Message = $"The given names do not match the name on the account."
                };
            }

            return await DepositAmount(accountNumber, amount);
        }

        public async Task<Result<decimal>> TransferAmount(int accountNumber, decimal amount)
        {
            // Get the account 
            Result<Account> getByAccountNumberResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (getByAccountNumberResult.Succeeded == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = getByAccountNumberResult.ResultType,
                    Message = $"Unable to deposit {amount} to account {accountNumber}. Reason: {getByAccountNumberResult.Message}."
                };
            }
            Account account = getByAccountNumberResult.Value;

            // Verify that we have enough balance on the account
            if (account.Amount < amount)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = getByAccountNumberResult.ResultType,
                    Message = $"Unable to transfer {amount} from account {accountNumber} - not enough balance, account balance {account.Amount}."
                };
            }

            // Update the account
            await _accountRepository.UpdateAccountAsync(account);

            // Return the updated balance of the account
            return new Result<decimal>()
            {
                Succeeded = true,
                Value = account.Amount
            };
        }

        public async Task<Result<decimal>> TransferAmount(int sourceAccountNumber, string sourcePin, int destAccountNumber, string destFirstName, string destLastName, decimal amount)
        {
            // Validate the destination account information
            if (Validate(destFirstName, destLastName, destAccountNumber) == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.InvalidData,
                    Message = $"Unable to transfer {amount} from {sourceAccountNumber} to {destAccountNumber}"
                };
            }

            // A transfer is a combination of a withdraw and a deposit
            Result<decimal> withDrawResult = await WithdrawAmount (sourceAccountNumber, sourcePin, amount);
            if (withDrawResult.Succeeded == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.InvalidData,
                    Message = $"Unable to transfer {amount} from {sourceAccountNumber} to {destAccountNumber}"
                };
            }

            Result<decimal> depositResult = await DepositAmount(destFirstName, destLastName, destAccountNumber, amount);
            if (depositResult.Succeeded == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.InvalidData,
                    Message = $"Unable to transfer {amount} from {sourceAccountNumber} to {destAccountNumber}"
                };
            }

            return withDrawResult;
        }

        public async Task<Result<decimal>> WithdrawAmount(int accountNumber, decimal amount)
        {
            // Get the account
            Result<Account> getByAccountNumberResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (getByAccountNumberResult.Succeeded == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.NotFound,
                    Message = $"Unable to withdraw {amount} from account {accountNumber} - account does not exist."
                };
            }
            Account account = getByAccountNumberResult.Value;

            // Verify that we have enough balance on the account
            if (account.Amount < amount)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.NotFound,
                    Message = $"Unable to withdraw {amount} from account {accountNumber} - not enough balance, account balance {account.Amount}."
                };
            }

            // Modify the balance of the account
            account.Amount = account.Amount - amount;

            // Update the account
            await _accountRepository.UpdateAccountAsync(account);

            // Return the updated balance of the account
            return new Result<decimal>()
            {
                Succeeded = true,
                Value = account.Amount
            };
        }

        public async Task<Result<decimal>> WithdrawAmount(int accountNumber, string pin, decimal amount)
        {
            // Verify the account number 
            Result<Account> getByAccountNumberResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (getByAccountNumberResult.Succeeded == false)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.NotFound,
                    Message = $"Unable to withdraw {amount} from account {accountNumber} - account does not exist."
                };
            }
            Account account = getByAccountNumberResult.Value;

            if (pin != account.Pin)
            {
                return new Result<decimal>()
                {
                    Succeeded = false,
                    ResultType = ResultType.InvalidData,
                    Message = "Incorrect Pin"
                };
            }
            return await WithdrawAmount(accountNumber, amount);
        }

        private async Task<bool> Validate(string firstName, string lastName, int accountNumber)
        {
            Result<Account> getByAccountNumberResult = await _accountRepository.GetByAccountNumberAsync(accountNumber);
            if (getByAccountNumberResult.Succeeded == false)
            {
                Console.WriteLine("This account does not exist.");
                return false;
            }

            Account account = getByAccountNumberResult.Value;

            Result<BankUser> user = await _userRepository.GetByUserNameAsync(account.UserName);
            if (firstName != user.Value.FirstName || lastName != user.Value.LastName)
            {
                Console.WriteLine("The names do not match the account number.");
                return false;
            }
            return true;
        }
    }
}
