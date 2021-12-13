using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ATMProject
{
    using DataAccess;
    using Models;
    using Accounts;
    using Number;
    using Logic;

    class ATM
    {
        static void Main(string[] args)
        {
            //language();
            /*
            IData dataAccessor = new Data();
            List<User> users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user = new User();
            user.UserName = "venom";
            user.FirstName = "Eddie";
            user.LastName = "Brock";

            dataAccessor.CreatAUser(user);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user2 = new User();
            user2.UserName = "deadpool";
            user2.FirstName = "Wade";
            user2.LastName = "Wilson";
            
            dataAccessor.CreatAUser(user2);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user3 = new User();
            user3.UserName = "elektra";
            user3.FirstName = "Elektra";
            user3.LastName = "Natchios";

            dataAccessor.CreatAUser(user3);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user4 = new User();
            user4.UserName = "blackwidow";
            user4.FirstName = "Natasha";
            user4.LastName = "Romanoff";

            dataAccessor.CreatAUser(user4);
            users = dataAccessor.GetAllUsers();
            PrintUsers(users);

            User user5 = new User();
            user5.UserName = "weaponx";
            user5.FirstName = "Logan";
            user5.LastName = "X";

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
            account.Amount = 4025.00M;
            
            accountAccessor.CreateAccount(account);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account2 = new Account();
            account2.UserName = "deadpool";
            account2.Number = 67890;
            account2.Amount = 535791.00M;

            accountAccessor.CreateAccount(account2);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account3 = new Account();
            account3.UserName = "elektra";
            account3.Number = 09876;
            account3.Amount = 50456.50M;

            accountAccessor.CreateAccount(account3);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account4 = new Account();
            account4.UserName = "blackwidow";
            account4.Number = 54321;
            account4.Amount = 623450.75M;

            accountAccessor.CreateAccount(account4);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            Account account5 = new Account();
            account5.UserName = "weaponX";
            account5.Number = 13579;
            account5.Amount = 62468.24M;

            accountAccessor.CreateAccount(account5);
            accounts = accountAccessor.GetAllAccounts();
            PrintAccounts(accounts);

            account4.Amount = 500000;
            accountAccessor.UpdateAccount(account4);

            user4.UserName = "bwidow";
            dataAccessor.UpdateUser(user4);

            user5.UserName = user4.UserName;
            dataAccessor.UpdateUser(user5);

            accountAccessor.DeleteAccount(account4);

            List<Account> userAccounts = accountAccessor.GetAllUserAccounts("blackwidow");
            List<User> allUsers = dataAccessor.GetAllUserNames("deadpool");
            List<User> allFirst = dataAccessor.GetAllFirstNames("Logan");
            List<User> allLast = dataAccessor.GetAllLastNames("X");
            */
            IAccountData accountAccessor = new AccountData();
            IAccountLogic accountLogic = new AccountLogic(accountAccessor);
            accountLogic.WithdrawAmount(13579, 320000000000000);

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
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("------------------------------");
            Console.WriteLine("1. CHECK BALANCE");
            Console.WriteLine("------------------------------");
            Console.WriteLine("2. DEPOSIT");
            Console.WriteLine("------------------------------");
            Console.WriteLine("3. WITHDRAW");
            Console.WriteLine("------------------------------");
            Console.WriteLine("4. TERMINATE TRANSACTION");

            var options = int.Parse(Console.ReadLine());

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
            var amount = 500;

            var answer = "";

            Console.WriteLine("YOUR CURRENT BALANCE IS: {0}\n", amount);
            Console.WriteLine("WOULD YOU LIKE TO MAKE ANOTHER TRANSACTION? ");
            answer = Console.ReadLine();

            if (answer == "y")
            {
                mainMenu();
            }
            else if (answer == "n")
            {
                exit();
            }
            else
            {
                exit();
            }
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
            var amount = 500.00;

            Console.WriteLine("HOW MUCH YOU WOULD LIKE TO DEPOSIT? ");
            var depoistAmount = double.Parse(Console.ReadLine());

            Console.WriteLine("YOUR NEW BALANCE IS: {0}", amount + depoistAmount);
            exit();
            
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
            var amount = 500;

            Console.WriteLine("HOW MUCH YOU WOULD LIKE TO WITHDRAW? ");
            Console.WriteLine("1. $20");
            Console.WriteLine("2. $40");
            Console.WriteLine("3. $60");
            Console.WriteLine("4. $80");
            Console.WriteLine("5. $100");
            Console.WriteLine("6. RETURN TO MAIN MENU");

            var withdrawOption = int.Parse(Console.ReadLine());

            switch (withdrawOption)
            {
                case 1:
                    Console.WriteLine("YOUR NEW BALANCE IS: {0}", amount - 20);
                    exit();
                    break;
                case 2:
                    Console.WriteLine("YOUR NEW BALANCE IS: {0}", amount - 40);
                    exit();
                    break;
                case 3:
                    Console.WriteLine("YOUR NEW BALANCE IS: {0}", amount - 60);
                    exit();
                    break;
                case 4:
                    Console.WriteLine("YOUR NEW BALANCE IS: {0}", amount - 80);
                    exit();
                    break;
                case 5:
                    Console.WriteLine("YOUR NEW BALANCE IS: {0}", amount - 100);
                    exit();
                    break;
                default:
                    exit();
                    break;
            }
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