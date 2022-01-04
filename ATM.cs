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

    class ATM
    {
        static void Main(string[] args)
        {
            IData dataAccessor = new Data();

            string userName;
            string password = "";
            string nombreUsuario;
            string contraseña = "";

            Console.WriteLine("Select a language:\n1. English\n2. Spanish");
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

                    if (password == user.Password)
                    {
                        Console.WriteLine("\nUser Authenticated.\nWhat would you like to do today?");
                        mainMenu();
                    }
                    else
                    {
                        Console.WriteLine("\nIncorrect Password. Please try again: ");
                    }
                }
            }
            else
            {
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
                    Console.WriteLine("Por favor introduzca su contraseña ");
                    contraseña = Console.ReadLine();

                    if (contraseña == usuario.Password)
                    {
                        Console.WriteLine("\nUsuario autenticado.\nQué te gustaría hacer hoy?");
                        menúPrincipal();
                    }
                    else
                    {
                        Console.WriteLine("\nContraseña incorrecta. Contraseña incorrecta: ");
                    }
                }
            }

            /*
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

        static void mainMenu()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("------------------------------");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("------------------------------");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("------------------------------");
            Console.WriteLine("4. Terminate Transaction");
            Console.WriteLine("------------------------------");

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
                    exit();
                    break;
                default:
                    Console.WriteLine("This is not a valid option.");
                    return;
            }
        }

        static void menúPrincipal()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("\nMenú principal");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. Consultar saldo");
            Console.WriteLine("------------------------------");
            Console.WriteLine("2. Depositar");
            Console.WriteLine("------------------------------");
            Console.WriteLine("3. Retirar");
            Console.WriteLine("------------------------------");
            Console.WriteLine("4. Terminar transacción");

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
                    salida();
                    break;
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
            var amountS = 500;

            var answerS = "";

            Console.WriteLine("SU SALDO ACTUAL ES: {0}\n", amountS);
            Console.WriteLine("DESEA REALIZR OTRA TRANSACCION?");
            answerS = Console.ReadLine();

            if (answerS == "y")
            {
                menúPrincipal();
            }
            else if (answerS == "n")
            {
                salida();
            }
            else
            {
                salida();
            }
        }

        static void deposit()
        {
            IAccountData accountAccessor = new AccountData();
            IData dataAccessor = new Data();

            int accountNumber;
            string firstName;
            string lastName;
            decimal amount;

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
            amount = decimal.Parse(Console.ReadLine());

            // Call deposit function
            IAccountLogic logicAccesssor = new AccountLogic(accountAccessor);

            logicAccesssor.DepositAmount(accountNumber, amount);

            Console.WriteLine("Your transaction was successful!");
            return;
        }

        static void depositar()
        {
            var amountS = 500.00;

            Console.WriteLine("CUANTO LE GUSTARIA DEPOSITAR? ");
            var depositAmountS = double.Parse(Console.ReadLine());

            Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS + depositAmountS);
            salida();
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
            var amountS = 500;

            Console.WriteLine("CUANTO LE GUSTARIA RETIRAR? ");
            Console.WriteLine("1. $20");
            Console.WriteLine("2. $40");
            Console.WriteLine("3. $60");
            Console.WriteLine("4. $80");
            Console.WriteLine("5. $100");
            Console.WriteLine("6. VOLVER AL MENU PRINCIPAL");

            var withdrawOptionS = int.Parse(Console.ReadLine());

            switch (withdrawOptionS)
            {
                case 1:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 20);
                    salida();
                    break;
                case 2:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 40);
                    salida();
                    break;
                case 3:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 60);
                    salida();
                    break;
                case 4:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 80);
                    salida();
                    break;
                case 5:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 100);
                    salida();
                    break;
                default:
                    salida();
                    break;
            }
        }

        static void exit()
        {
            Console.WriteLine("Thank you for using CyberBank. GoodBye.");
        }

        static void salida()
        {
            Console.WriteLine("GRACIAS POR USAR EL ADIOS DE CYBERBANK.");
        }
    }
}
