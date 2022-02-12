namespace OnlineBankingProject
{
    using System;
    using System.Collections.Generic;
    using DataAccess;
    using Models;
    using Logic;
    using global::Bank;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.Extensions.DependencyInjection;

    class OnlineBankingProject
    {
        private static IUserRepository _userRepository;
        private static IAccountRepository _accountRepository;
        private static IAccountLogic _accountLogic;

        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = ConfigureSerivces();

            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _accountRepository = serviceProvider.GetRequiredService<IAccountRepository>();
            _accountLogic = serviceProvider.GetRequiredService<IAccountLogic>();

            string userName;
            string password = "";

            // Ask for the username and verify
            Console.WriteLine("Please enter your username: ");
            userName = Console.ReadLine();

            Result<BankUser> user = _userRepository.GetByUserNameAsync(userName);

            if (user.Succeeded == false)
            {
                Console.WriteLine("This user does not exist.");
                return;
            }

            // Ask for the password and verify
            while (password != user.Value.Password)
            {
                Console.WriteLine("\nPlease enter your password: ");
                password = Console.ReadLine();

                // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }
                Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
                Console.WriteLine($"Hashed: {hashed}");

                if (password == user.Value.Password)
                {
                    Console.WriteLine("\nUser Authenticated.\nWhat would you like to do today?\n");
                    mainMenu(userName);
                }
                else
                {
                    Console.WriteLine("\nIncorrect Password. Please try again: ");
                }
            }
        }

        static private void PrintUsers(List<BankUser> users)
        {
            Console.WriteLine($"Number of users: {users.Count}");
            foreach (BankUser user in users)
            {
                Console.WriteLine($"Id: {user.Id} UserName: {user.UserName}, FirstName: {user.FirstName}, LastName: {user.LastName}");
            }
            Console.WriteLine("");
        }

        static private void PrintAccounts(List<Account> accounts)
        {
            Console.WriteLine($"Number of accounts: {accounts.Count}");
            foreach (Account account in accounts)
            {
                Console.WriteLine($"UserName: {account.UserName}, Number: {account.Number}, Amount: {account.Amount}");
            }
            Console.WriteLine("");
        }

        static void mainMenu(string userName)
        {
            while (true)
            {


                Console.WriteLine("------------------------------");
                Console.WriteLine("Main Menu");
                Console.WriteLine("------------------------------");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("------------------------------");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("------------------------------");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("------------------------------");
                Console.WriteLine("4. Transfer");
                Console.WriteLine("------------------------------");
                Console.WriteLine("5. View All Accounts");
                Console.WriteLine("------------------------------");
                Console.WriteLine("6. Terminate Transaction");
                Console.WriteLine("------------------------------\n");

                int options = int.Parse(Console.ReadLine());

                switch (options)
                {
                    case 1:
                        balance();
                        break;
                    case 2:
                        deposit();
                        break;
                    case 3:
                        withdraw();
                        break;
                    case 4:
                        transfer();
                        break;
                    case 5:
                        viewAccounts(userName);
                        break;
                    case 6:
                        exit();
                        return;
                    default:
                        Console.WriteLine("This is not a valid option.");
                        return;
                }
            }
        }

        static void balance()
        {
            int accountNumber;

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number
            Result<Account> account = _accountRepository.GetByAccountNumberAsync(accountNumber).re;

            if (account == null)
            {
                Console.WriteLine("This account does not exist.");
                return;
            }

            Console.WriteLine($"\nYour current balance is: {account.Value.Amount}");
            return;
        }

        static void deposit()
        {
      
            int accountNumber;
            string firstName;
            string lastName;
            decimal depositAmount;

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Ask for the first name 
            Console.WriteLine("Please enter your first name: ");
            firstName = Console.ReadLine();

            // Ask for the last name 
            Console.WriteLine("Please enter your last name: ");
            lastName = Console.ReadLine();

            // Ask for the amount 
            Console.WriteLine("Please enter the deposit amount: ");
            depositAmount = decimal.Parse(Console.ReadLine());

            // Call deposit function
            _accountLogic.DepositAmount(firstName, lastName, accountNumber, depositAmount);
            return;
        }

        static void withdraw()
        {
            int accountNumber;
            decimal withdrawAmount;
            string pin = "";

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Ask for pin
            Console.WriteLine("Please enter your pin: ");
            pin = Console.ReadLine();

            // Ask for withdraw amount
            Console.WriteLine("How much would you like to withdraw? ");
            Console.WriteLine("1. $20");
            Console.WriteLine("2. $40");
            Console.WriteLine("3. $60");
            Console.WriteLine("4. $80");
            Console.WriteLine("5. $100");
            Console.WriteLine("6. Enter your amount");

            int withdrawOption = int.Parse(Console.ReadLine());

            switch (withdrawOption)
            {
                case 1:
                    withdrawAmount = 20;
                    exit();
                    break;
                case 2:
                    withdrawAmount = 40;
                    exit();
                    break;
                case 3:
                    withdrawAmount = 60;
                    exit();
                    break;
                case 4:
                    withdrawAmount = 80;
                    exit();
                    break;
                case 5:
                    withdrawAmount = 100;
                    exit();
                    break;
                case 6:
                    Console.WriteLine("Please enter an amount: ");
                    withdrawAmount = decimal.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Invalid Option!");
                    return;
            }

            _accountLogic.WithdrawAmount(accountNumber, pin, withdrawAmount);
            return;
        }

        static void transfer()
        {
            int sourceAccount;
            int destinationAccount;
            decimal transferAmount;
            string pin = "";
            string firstName;
            string lastName;

            Console.WriteLine("Enter the account number you want to transfer from: ");
            sourceAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter your pin: ");
            pin = Console.ReadLine();

            Console.WriteLine("\nPlease enter the amount you are transferring: ");
            transferAmount = decimal.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter the account you want to transfer to: ");
            destinationAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter the first name of the account holder: ");
            firstName = Console.ReadLine();

            Console.WriteLine("\nPlease enter the last name of the account holder: ");
            lastName = Console.ReadLine();

            _accountLogic.TransferAmount(sourceAccount, pin, destinationAccount, firstName, lastName, transferAmount);
            return;
        }

        static void viewAccounts(string userName)
        {
            Result<List<Account>> accounts = _accountRepository.GetAllByUsernameAsync(userName).Result;

            PrintAccounts(accounts.Value);   
        }

        static void exit()
        {
            Console.WriteLine("Thank you for using CyberBank. GoodBye.");
            return;
        }

        /// <summary>
        /// Configure the dependency injection 
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ConfigureSerivces()
        {
            IServiceCollection services = new ServiceCollection();

            // Define implementations of interfaces
            services.AddSingleton<IAccountRepository, AccountsCosmosDbRepository>();
            services.AddSingleton<IUserRepository, UsersCosmosDbRepository>();
            services.AddSingleton<IAccountLogic, AccountLogic>();

            // Build the service provider
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
