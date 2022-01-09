namespace ATMProject
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using DataAccess;
    using Models;
    using Accounts;
    using Number;
    using Logic;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    class ATM
    {
        static void Main(string[] args)
        {
            IData dataAccessor = new Data();

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

            /*
            // Log user data into json file 
            IData dataAccessor = new Data();
            List<User> users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user = new User();
            user.UserName = "venom";
            user.FirstName = "Eddie";
            user.LastName = "Brock";
            user.Password = "password";

            dataAccessor.CreatAUser(user);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user2 = new User();
            user2.UserName = "deadpool";
            user2.FirstName = "Wade";
            user2.LastName = "Wilson";
            user2.Password = "password";
            
            dataAccessor.CreatAUser(user2);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user3 = new User();
            user3.UserName = "elektra";
            user3.FirstName = "Elektra";
            user3.LastName = "Natchios";
            user3.Password = "password";

            dataAccessor.CreatAUser(user3);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user4 = new User();
            user4.UserName = "blackwidow";
            user4.FirstName = "Natasha";
            user4.LastName = "Romanoff";
            user4.Password = "password";

            dataAccessor.CreatAUser(user4);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user5 = new User();
            user5.UserName = "weaponx";
            user5.FirstName = "Logan";
            user5.LastName = "X";
            user5.Password = "password";

            dataAccessor.CreatAUser(user5);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            Console.WriteLine("done");

            // Log account data into json file
            IAccountData accountAccessor = new AccountData();
            List<Account> accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account = new Account();
            account.UserName = "venom";
            account.Number = 12345;
            account.Amount = 4025;
            account.Pin = "1234";
            
            accountAccessor.CreateAccount(account);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account2 = new Account();
            account2.UserName = "deadpool";
            account2.Number = 67890;
            account2.Amount = 535791;
            account2.Pin = "1234";

            accountAccessor.CreateAccount(account2);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account3 = new Account();
            account3.UserName = "elektra";
            account3.Number = 09876;
            account3.Amount = 50456;
            account3.Pin = "1234";

            accountAccessor.CreateAccount(account3);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account4 = new Account();
            account4.UserName = "blackwidow";
            account4.Number = 54321;
            account4.Amount = 623450;
            account4.Pin = "1234";

            accountAccessor.CreateAccount(account4);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account5 = new Account();
            account5.UserName = "weaponX";
            account5.Number = 13579;
            account5.Amount = 62468;
            account5.Pin = "1234";

            accountAccessor.CreateAccount(account5);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            //account4.Amount = 20000;
            //accountAccessor.UpdateAccount(account4);

            //user4.UserName = "bwidow";
            //dataAccessor.UpdateUser(user4);

            //user5.UserName = user4.UserName;
            //dataAccessor.UpdateUser(user5);

            //accountAccessor.DeleteAccount(account4);

            List<Account> userAccounts = accountAccessor.GetAllUserAccounts("blackwidow");
            List<User> allUsers = dataAccessor.GetAllUserNames("deadpool");
            List<User> allFirst = dataAccessor.GetAllFirstNames("Logan");
            List<User> allLast = dataAccessor.GetAllLastNames("X");
            */

            Console.WriteLine("done");

            //Console.ReadKey();
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
            Console.WriteLine("4. Ver todas las cuentas");
            Console.WriteLine("------------------------------");
            Console.WriteLine("5. Terminar transacción");
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
                    verCuentas(nombreUsario);
                    break;
                case 5:
                    salida();
                    return;
                default:
                    Console.WriteLine("Esta no es una opción válida.");
                    return;

            }
        }

        static void balance()
        {
            IAccountData accountAccessor = new AccountData();

            int accountNumber;

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number
            Account account = accountAccessor.GetByAccountNumber(accountNumber);

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
            IAccountData accountAccessor = new AccountData();

            int accountNumber;

            // Ask for the account number 
            Console.WriteLine("Ingrese su número de cuenta: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number
            Account account = accountAccessor.GetByAccountNumber(accountNumber);

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
            IAccountData accountAccessor = new AccountData();
            IData dataAccessor = new Data();
            IAccountLogic logic = new AccountLogic(accountAccessor);

            int accountNumber;
            string firstName;
            string lastName;
            decimal depositAmount;

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            Account account = accountAccessor.GetByAccountNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("This account does not exist.");
                return;
            }

            // Ask for the first name 
            Console.WriteLine("Please enter your first name: ");
            firstName = Console.ReadLine();

            // Ask for the last name 
            Console.WriteLine("Please enter your last name: ");
            lastName = Console.ReadLine();

            // Verify first and last name 
            User user = dataAccessor.GetByUserName(account.UserName);

            if (firstName != user.FirstName || lastName != user.LastName)
            {
                Console.WriteLine("The names do not match the account number.");
                return;
            }

            // Ask for the amount 
            Console.WriteLine("Please enter the deposit amount: ");
            depositAmount = decimal.Parse(Console.ReadLine());

            // Call deposit function
            logic.DepositAmount(accountNumber, depositAmount);

            Console.WriteLine("Your transaction was successful!");
            return;
        }

        static void depositar()
        {
            IAccountData accountAccessor = new AccountData();
            IData dataAccessor = new Data();
            IAccountLogic logic = new AccountLogic(accountAccessor);

            int accountNumber;
            string firstName;
            string lastName;
            decimal depositAmount;

            // Ask for the account number 
            Console.WriteLine("Ingrese su número de cuenta: ");
            accountNumber = int.Parse(Console.ReadLine());

            Account account = accountAccessor.GetByAccountNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Esta cuenta no existe.");
                return;
            }

            // Ask for the first name 
            Console.WriteLine("Por favor, introduzca su nombre de pila: ");
            firstName = Console.ReadLine();

            // Ask for the last name 
            Console.WriteLine("Por favor ingrese su apellido: ");
            lastName = Console.ReadLine();

            // Verify first and last name 
            User user = dataAccessor.GetByUserName(account.UserName);

            if (firstName != user.FirstName || lastName != user.LastName)
            {
                Console.WriteLine("Los nombres no coinciden con el número de cuenta.");
                return;
            }

            // Ask for the amount 
            Console.WriteLine("Ingrese el monto del depósito: ");
            depositAmount = decimal.Parse(Console.ReadLine());

            // Call deposit function
            logic.DepositAmount(accountNumber, depositAmount);

            Console.WriteLine("Tu transacción fue exitosa!");
            return;
        }

        static void withdraw()
        {
            IAccountData accountAccessor = new AccountData();
            IAccountLogic logic = new AccountLogic(accountAccessor);

            int accountNumber;
            decimal withdrawAmount;
            string pin = "";

            // Ask for the account number 
            Console.WriteLine("Please enter your account number: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number 
            Account account = accountAccessor.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine("This account does not exist.");
                return;
            }

            // Ask for the pin
            while (pin != account.Pin)
            {
                Console.WriteLine("Please enter your pin: ");
                pin = Console.ReadLine();

                if (pin == account.Pin)
                {
                    Console.WriteLine("User Authenticated.\n");
                }
                else
                {
                    Console.WriteLine("Incorrect pin. Please try again: ");
                }
            }

            // Complete withdraw 
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

            logic.WithdrawAmount(accountNumber, withdrawAmount);
            
            Console.WriteLine("Your transaction was successful!");
            return;
        }

        static void retirar()
        {
            IAccountData accountAccessor = new AccountData();
            IAccountLogic logic = new AccountLogic(accountAccessor);

            int accountNumber;
            decimal withdrawAmount;
            string pin = "";

            // Ask for the account number 
            Console.WriteLine("Ingrese su número de cuenta: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number 
            Account account = accountAccessor.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Esta cuenta no existe.");
                return;
            }

            // Ask for the pin
            while (pin != account.Pin)
            {
                Console.WriteLine("Por favor ingrese su pin: ");
                pin = Console.ReadLine();

                if (pin == account.Pin)
                {
                    Console.WriteLine("Usuario autenticado.\n");
                }
                else
                {
                    Console.WriteLine("Pin incorrecto. Inténtalo de nuevo: ");
                }
            }

            // Complete withdraw 
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

            logic.WithdrawAmount(accountNumber, withdrawAmount);

            Console.WriteLine("Tu transacción fue exitosa!");
            return;
        }

        static void transfer()
        {
            IAccountData accountAccessor = new AccountData();
            IData dataAccessor = new Data();
            IAccountLogic logic = new AccountLogic(accountAccessor);

            int sourceAccount;
            int destinationAccount;
            decimal transferAmount;
            string pin = "";
            string firstName;
            string lastName;

            Console.WriteLine("Enter the account number you want to transfer from: ");
            sourceAccount = int.Parse(Console.ReadLine());

            Account account1 = accountAccessor.GetByAccountNumber(sourceAccount);

            if (account1 == null)
            {
                Console.WriteLine("This account does not exist!");
                return;
            }

            while (pin != account1.Pin)
            {
                Console.WriteLine("\nPlease enter your pin: ");
                pin = Console.ReadLine();

                if (pin == account1.Pin)
                {
                    Console.WriteLine("\nUser Authenticated.");
                }
                else
                {
                    Console.WriteLine("\nIncorrect pin. Please try again: ");
                }
            }

            Console.WriteLine("\nPlease enter the amount you are transferring: ");
            transferAmount = decimal.Parse(Console.ReadLine());

            Console.WriteLine("\nPlease enter the account you want to transfer to: ");
            destinationAccount = int.Parse(Console.ReadLine());

            Account account2 = accountAccessor.GetByAccountNumber(destinationAccount);
            
            if (account2 == null)
            {
                Console.WriteLine("This account does not exist!");
                return;
            }

            Console.WriteLine("\nPlease enter the first name of the account holder: ");
            firstName = Console.ReadLine();

            Console.WriteLine("\nPlease enter the last name of the account holder: ");
            lastName = Console.ReadLine();

            User user = dataAccessor.GetByUserName(account2.UserName);

            if (firstName != user.FirstName || lastName != user.LastName)
            {
                Console.WriteLine("The names do not match the account number.");
                return;
            }
            
            transferAmount = logic.WithdrawAmount(sourceAccount, transferAmount) + logic.DepositAmount(destinationAccount, transferAmount);

            Console.WriteLine("Your transfer was successful!");
            return;
        }

        static void viewAccounts(string userName)
        {
            IAccountData accountAccessor = new AccountData();

            List<Account> accounts = accountAccessor.GetAllUserAccounts(userName);

            PrintAccounts(accounts);   
        }

        static void verCuentas(string nombreUsario)
        {
            IAccountData accountAccessor = new AccountData();

            List<Account> accounts = accountAccessor.GetAllUserAccounts(nombreUsario);

            PrintAccounts(accounts);
        }

        static void exit()
        {
            Console.WriteLine("Thank you for using CyberBank. GoodBye.");
        }

        static void salida()
        {
            Console.WriteLine("Gracias por utilizar CyberBank. Adiós.");
        }
    }
}
