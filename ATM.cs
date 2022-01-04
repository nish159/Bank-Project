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

            // Ask for the username and verify
            Console.WriteLine("WELCOME TO CYBERBANK. PLEASE ENTER YOUR USERNAME: ");
            userName = Console.ReadLine();

            User user = dataAccessor.GetByUserName(userName);

            if (user == null)
            {
                Console.WriteLine("THIS USER DOES NOT EXIST IN OUR SYSTEMS.");
                return;
            }
            // Ask for the password and verify
            while (password != user.Password)
            {
                Console.WriteLine("\nPLEASE ENTER YOUR PASSWORD: ");
                password = Console.ReadLine();

                if (password == user.Password)
                {
                    Console.WriteLine("\nUSER AUTHENTICATED.\nWHAT WOULD YOU LIKE TO DO TODAY?");
                    mainMenu();
                }
                else
                {
                    Console.WriteLine("\nINCORRECT PASSWORD. PLEASE TRY AGAIN: ");
                }
            }

            //language();

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

        static void language()
        {
            Console.WriteLine("SELECT A LANGUAGE:\n1. ENGLISH\n2. SPANISH");
            var languageOption = int.Parse(Console.ReadLine());

            if (languageOption == 1)
            {
                Console.WriteLine("WELCOME TO CYBERBANK\n");
                pin();

                Console.WriteLine("WHAT WOULD YOU LIKE TO DO TODAY?");
                mainMenu();
            }
            else
            {
                Console.WriteLine("BIENVENIDO A CYBERBANK\n");
                pinS();

                Console.WriteLine("QUE TE GUSTARIA HACER HOY?");
                mainMenuS();
            }
        }

        static void pin()
        {

            var pin = "";

            while (pin != "1234")
            {
                Console.WriteLine("PLEASE ENTER YOUR PIN: ");
                pin = Console.ReadLine();

                if (pin == "1234")
                {
                    Console.WriteLine("\nAUTHENTICATED\n");
                }
                else
                {
                    Console.WriteLine("\nNOT AUTHENTICATED\n");
                }
            }
        }

        static void pinS()
        {
            var pinS = "";

            while (pinS != "1234")
            {
                Console.WriteLine("INTRODUCE TU PIN: ");
                pinS = Console.ReadLine();

                if (pinS == "1234")
                {
                    Console.WriteLine("\nAUTENTICADO\n");
                }
                else
                {
                    Console.WriteLine("\nSIN AUTENTICADO\n");
                }
            }
        }

        static void mainMenu()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. CHECK BALANCE");
            Console.WriteLine("------------------------------");
            Console.WriteLine("2. DEPOSIT");
            Console.WriteLine("------------------------------");
            Console.WriteLine("3. WITHDRAW");
            Console.WriteLine("------------------------------");
            Console.WriteLine("4. TERMINATE TRANSACTION");
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
            }
        }

        static void mainMenuS()
        {
            Console.WriteLine("\nMENU PRINCIPAL");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. COMPROBAR SALDO");
            Console.WriteLine("------------------------------");
            Console.WriteLine("2. DEPOSITO");
            Console.WriteLine("------------------------------");
            Console.WriteLine("3. RETIRAR");
            Console.WriteLine("------------------------------");
            Console.WriteLine("4. FINALIZAR TRANSACCION");

            var optionsS = int.Parse(Console.ReadLine());

            switch (optionsS)
            {
                case 1:
                    balanceS();
                    break;
                case 2:
                    depositS();
                    break;
                case 3:
                    withdrawS();
                    break;
                case 4:
                    exitS();
                    break;

            }
        }

        static void balance()
        {
            IAccountData accountAccessor = new AccountData();

            int accountNumber;

            // Ask for the account number 
            Console.WriteLine("PLEASE ENTER YOUR ACCOUNT NUMBER: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number
            Account account = accountAccessor.GetByAccountNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("THIS ACCOUNT DOES NOT EXIST.");
                return;
            }

            Console.WriteLine($"\nYOUR CURRENT BALANCE IS: {account.Amount}");
            return;
        }

        static void balanceS()
        {
            var amountS = 500;

            var answerS = "";

            Console.WriteLine("SU SALDO ACTUAL ES: {0}\n", amountS);
            Console.WriteLine("DESEA REALIZR OTRA TRANSACCION?");
            answerS = Console.ReadLine();

            if (answerS == "y")
            {
                mainMenuS();
            }
            else if (answerS == "n")
            {
                exitS();
            }
            else
            {
                exitS();
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
            Console.WriteLine("PLEASE ENTER YOUR ACCOUNT NUMBER: ");
            accountNumber = int.Parse(Console.ReadLine());

            Account account = accountAccessor.GetByAccountNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("THIS ACCOUNT DOES NOT EXIST.");
                return;
            }

            // Ask for the first name 
            Console.WriteLine("PLEASE ENTER YOUR FIRST NAME: ");
            firstName = Console.ReadLine();

            // Ask for the last name 
            Console.WriteLine("PLEASE ENTER YOUR LAST NAME: ");
            lastName = Console.ReadLine();

            // Verify first and last name 
            User user = dataAccessor.GetByUserName(account.UserName);

            if (firstName != user.FirstName || lastName != user.LastName)
            {
                Console.WriteLine("THE NAMES DO MATCH THE ACCOUNT.");
                return;
            }

            // Ask for the amount 
            Console.WriteLine("PLEASE ENTER THE DEPOSIT AMOUNT: ");
            amount = decimal.Parse(Console.ReadLine());

            // Call deposit function
            IAccountLogic logicAccesssor = new AccountLogic(accountAccessor);

            logicAccesssor.DepositAmount(accountNumber, amount);

            Console.WriteLine("YOUR TRANSACTION WAS SUCCESSFUL!");
            return;
        }

        static void depositS()
        {
            var amountS = 500.00;

            Console.WriteLine("CUANTO LE GUSTARIA DEPOSITAR? ");
            var depositAmountS = double.Parse(Console.ReadLine());

            Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS + depositAmountS);
            exitS();
        }

        static void withdraw()
        {
            IAccountData accountAccessor = new AccountData();
            IAccountLogic logic = new AccountLogic(accountAccessor);

            int accountNumber;
            decimal withdrawAmount;
            string pin = "";

            // Ask for the account number 
            Console.WriteLine("PLEASE ENTER YOUR ACCOUNT NUMBER: ");
            accountNumber = int.Parse(Console.ReadLine());

            // Verify the account number 
            Account account = accountAccessor.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine("THIS ACCOUNT IS NOT IN OUR SYSTEM.");
                return;
            }

            // Ask for the pin
            while (pin != account.Pin)
            {
                Console.WriteLine("PLEASE ENTER YOUR PIN: ");
                pin = Console.ReadLine();

                if (pin == account.Pin)
                {
                    Console.WriteLine("USER AUTHENTICATED\n");
                }
                else
                {
                    Console.WriteLine("INCORRECT PIN. PLEASE TRY AGAIN: ");
                }
            }

            // Complete withdraw 
            Console.WriteLine("HOW MUCH YOU WOULD LIKE TO WITHDRAW? ");
            Console.WriteLine("1. $20");
            Console.WriteLine("2. $40");
            Console.WriteLine("3. $60");
            Console.WriteLine("4. $80");
            Console.WriteLine("5. $100");
            Console.WriteLine("6. ENTER AN AMOUNT");

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
                    Console.WriteLine("PLEASE ENTER AN AMOUNT: ");
                    withdrawAmount = decimal.Parse(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("INVALID OPTION!");
                    return;
            }

            logic.WithdrawAmount(accountNumber, withdrawAmount);
            
            Console.WriteLine("YOUR TRANSACTION WAS SUCCESSFUL!");
            return;
        }

        static void withdrawS()
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
                    exitS();
                    break;
                case 2:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 40);
                    exitS();
                    break;
                case 3:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 60);
                    exitS();
                    break;
                case 4:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 80);
                    exitS();
                    break;
                case 5:
                    Console.WriteLine("SU SALDO ACTUAL ES: {0}", amountS - 100);
                    exitS();
                    break;
                default:
                    exitS();
                    break;
            }
        }

        static void exit()
        {
            Console.WriteLine("THANK YOU FOR USING CYBERBANK GOODBYE.");
        }

        static void exitS()
        {
            Console.WriteLine("GRACIAS POR USAR EL ADIOS DE CYBERBANK.");
        }
    }
}
