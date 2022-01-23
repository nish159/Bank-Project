namespace ATMProject
{
    using System;
    using System.Collections.Generic;
    using DataAccess;
    using Models;
    using Logic;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    class ATM
    {
        static void Main(string[] args)
        {
            IUserRepository dataAccessor = new UserFileSystemRepository();

            string userName;
            string password = "";
            string nombreUsuario;
            string contraseña = "";

            Console.WriteLine("Select a language:\n1. English\n2. Spanish\n");
            int languageOption = int.Parse(Console.ReadLine());

            if (languageOption == 1)
            {
                // Ask for the username and verify
                Console.WriteLine("Please enter your username: ");
                userName = Console.ReadLine();

                User user = dataAccessor.GetByUserName(userName);

                if (user == null)
                {
                    Console.WriteLine("This user does not exist.");
                    return;
                }

                // Ask for the password and verify
                while (password != user.Password)
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

                    if (password == user.Password)
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
            else
            {
                // Line of code to be executed when spanish option is selected
                Console.WriteLine("Por favor introduzca su nombre de usuario: ");
                nombreUsuario = Console.ReadLine();

                User usuario = dataAccessor.GetByUserName(nombreUsuario);

                if (nombreUsuario == null)
                {
                    Console.WriteLine("Este usuario no existe.");
                    return;
                }

                // Ask for the password and verify
                while (contraseña != usuario.Password)
                {
                    Console.WriteLine("Por favor introduzca su contraseña: ");
                    contraseña = Console.ReadLine();

                    if (contraseña == usuario.Password)
                    {
                        Console.WriteLine("\nUsuario autenticado.\n¿Qué te gustaría hacer hoy?\n");
                        menúPrincipal(nombreUsuario);
                    }
                    else
                    {
                        Console.WriteLine("\nContraseña incorrecta. Inténtalo de nuevo: ");
                    }
                }
            }
        }

        static private void PrintUsers(List<User> users)
        {
            Console.WriteLine($"Number of users: {users.Count}");
            foreach (User user in users)
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

        static void menúPrincipal(string nombreUsario)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Menú principal");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. Consultar saldo");
            Console.WriteLine("------------------------------");
            Console.WriteLine("2. Depositar");
            Console.WriteLine("------------------------------");
            Console.WriteLine("3. Retirar");
            Console.WriteLine("------------------------------");
            Console.WriteLine("4. Transferir");
            Console.WriteLine("------------------------------");
            Console.WriteLine("5. Ver todas las cuentas");
            Console.WriteLine("------------------------------");
            Console.WriteLine("6. Terminar transacción");
            Console.WriteLine("------------------------------\n");

            int opciones = int.Parse(Console.ReadLine());

            switch (opciones)
            {
                case 1:
                    equilibrio();
                    break;
                case 2:
                    depositar();
                    break;
                case 3:
                    retirar();
                    break;
                case 4:
                    transferir();
                    break;
                case 5:
                    verCuentas(nombreUsario);
                    break;
                case 6:
                    salida();
                    return;
                default:
                    Console.WriteLine("Esta no es una opción válida.");
                    return;

            }
        }

        static void balance()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();

            int accountNumber;

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number
            Account account = accountRepository.GetByAccountNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("This account does not exist.");
                return;
            }

            Console.WriteLine($"\nYour current balance is: {account.Amount}");
            return;
        }

        static void equilibrio()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();

            int accountNumber;

            // Ask for the account number 
            Console.WriteLine("Ingrese su número de cuenta: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number
            Account account = accountRepository.GetByAccountNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Esta cuenta no existe.");
                return;
            }

            Console.WriteLine($"\nEl saldo de su cuenta es: {account.Amount}");
            return;
        }

        static void deposit()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();
            IUserRepository userRepository = new UserFileSystemRepository();
            IAccountLogic logic = new AccountLogic(accountRepository, userRepository);

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
            logic.DepositAmount(firstName, lastName, accountNumber, depositAmount);
            return;
        }

        static void depositar()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();
            IUserRepository userRepository = new UserFileSystemRepository();
            IAccountLogic logic = new AccountLogic(accountRepository, userRepository);

            int accountNumber;
            string firstName;
            string lastName;
            decimal depositAmount;

            // Ask for the account number 
            Console.WriteLine("Ingrese su número de cuenta: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Ask for the first name 
            Console.WriteLine("Por favor, introduzca su nombre de pila: ");
            firstName = Console.ReadLine();

            // Ask for the last name 
            Console.WriteLine("Por favor ingrese su apellido: ");
            lastName = Console.ReadLine();

            // Ask for the amount 
            Console.WriteLine("Ingrese el monto del depósito: ");
            depositAmount = decimal.Parse(Console.ReadLine());

            // Call deposit function
            logic.DepositAmount(firstName, lastName, accountNumber, depositAmount);
            return;
        }

        static void withdraw()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();
            IUserRepository userRepository = new UserFileSystemRepository();
            IAccountLogic logic = new AccountLogic(accountRepository, userRepository);

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

            logic.WithdrawAmount(accountNumber, pin, withdrawAmount);
            return;
        }

        static void retirar()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();
            IUserRepository userRepository = new UserFileSystemRepository();
            IAccountLogic logic = new AccountLogic(accountRepository, userRepository);

            int accountNumber;
            decimal withdrawAmount;
            string pin = "";

            // Ask for the account number 
            Console.WriteLine("Ingrese su número de cuenta: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Ask for the pin
            Console.WriteLine("Por favor ingrese su pin: ");
            pin = Console.ReadLine();

            // Ask for withdraw amount
            Console.WriteLine("¿Cuánto le gustaría retirar?");
            Console.WriteLine("1. $20");
            Console.WriteLine("2. $40");
            Console.WriteLine("3. $60");
            Console.WriteLine("4. $80");
            Console.WriteLine("5. $100");
            Console.WriteLine("6. Ingrese su monto");

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
                    Console.WriteLine("Por favor ingrese una cantidad: ");
                    withdrawAmount = decimal.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Opción inválida!");
                    return;
            }

            logic.WithdrawAmount(accountNumber, pin, withdrawAmount);
            return;
        }

        static void transfer()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();
            IUserRepository userRepository = new UserFileSystemRepository();
            IAccountLogic logic = new AccountLogic(accountRepository, userRepository);

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

            logic.TransferAmount(sourceAccount, pin, destinationAccount, firstName, lastName, transferAmount);
            return;
        }

        static void transferir()
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();
            IUserRepository userRepository = new UserFileSystemRepository();
            IAccountLogic logic = new AccountLogic(accountRepository, userRepository);

            int sourceAccount;
            int destinationAccount;
            decimal transferAmount;
            string pin = "";
            string firstName;
            string lastName;

            Console.WriteLine("Ingrese el número de cuenta desde el que desea transferir: ");
            sourceAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPor favor ingrese su pin: ");
            pin = Console.ReadLine();

            Console.WriteLine("\nPlease enter the amount you are transferring: ");
            transferAmount = decimal.Parse(Console.ReadLine());

            Console.WriteLine("\nPor favor ingrese la cantidad que está transfiriendo: ");
            destinationAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("\nPor favor ingrese el nombre del titular de la cuenta: ");
            firstName = Console.ReadLine();

            Console.WriteLine("\nPor favor ingrese el apellido del titular de la cuenta: ");
            lastName = Console.ReadLine();

            logic.TransferAmount(sourceAccount, pin, destinationAccount, firstName, lastName, transferAmount);
            return;
        }

        static void viewAccounts(string userName)
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();

            List<Account> accounts = accountRepository.GetAllUserAccounts(userName);

            PrintAccounts(accounts);   
        }

        static void verCuentas(string nombreUsario)
        {
            IAccountRepository accountRepository = new AccountFileSystemRepository();

            List<Account> accounts = accountRepository.GetAllUserAccounts(nombreUsario);

            PrintAccounts(accounts);
        }

        static void exit()
        {
            Console.WriteLine("Thank you for using CyberBank. GoodBye.");
            return;
        }

        static void salida()
        {
            Console.WriteLine("Gracias por utilizar CyberBank. Adiós.");
            return;
        }
    }
}
