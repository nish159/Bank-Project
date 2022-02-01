using Bank;
using Microsoft.Azure.Cosmos;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    /// <summary>
    /// Defines the interface for interacting with the account data store.
    /// </summary>
    /// 

    public class AccountsCosmosDbRepository : IAccountRepository
    {
        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        // The name of the database and container we will create
        private const string _databaseId = "Accounts";
        private const string _containerId = "accounts";

        public AccountsCosmosDbRepository()
        {
            string endpointUri = "https://bankingdb.documents.azure.com:443/";
            string primaryKey = "xfSTXbXQHEUe6D7rTtLdpZ8QXMz8qxuSsJfwiAtWapSPRM7olhT7aAvcgSkDVyAIsO1wVhe5Uxhf0GYVvbW58g==";

            _cosmosClient = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = "BankProject" });
            _database = _cosmosClient.GetDatabase(_databaseId);
            _container = _database.GetContainer(_containerId);
        }

        public Result<Account> CreateAccount(Account account)
        {
            throw new System.NotImplementedException();
        }

        public Result<Account> DeleteAccount(Account deletedAccount)
        {
            throw new System.NotImplementedException();
        }

        public Result<List<Account>> GetAllAccounts()
        {
            throw new System.NotImplementedException();
        }

        public Result<List<Account>> GetAllByUsername(string userName)
        {
            // Building the sql query
            var sqlQueryText = $"SELECT * FROM c WHERE c.UserName = \"{userName}\"";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);



            // Querying the container
            FeedIterator<Account> queryResultSetIterator = _container.GetItemQueryIterator<Account>(queryDefinition);



            // Getting the results from the query
            List<Account> accounts = new List<Account>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Account> currentResultSet = queryResultSetIterator.ReadNextAsync().Result;
                foreach (Account account in currentResultSet)
                {
                    accounts.Add(account);
                }
            }



            // Check if the operation returned any accounts
            if (!accounts.Any())
            {
                return new Result<List<Account>>()
                {
                    Succeeded = false,
                    ResultType = ResultType.NotFound
                };
            }



            // The query returned a list of accounts
            return new Result<List<Account>>()
            {
                Succeeded = true,
                Value = accounts
            };
        }

        public Result<Account> GetByAccountNumber(int accountNumber)
        {
            throw new System.NotImplementedException();
        }

        public Result<Account> GetById(string id)
        {
            // Building the sql query
            var sqlQueryText = $"SELECT * FROM c WHERE c.Id = \"{id}\"";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            // Querying the container
            FeedIterator<Account> queryResultSetIterator = _container.GetItemQueryIterator<Account>(queryDefinition);


            // Getting the results from the query
            FeedResponse<Account> currentResultSet = queryResultSetIterator.ReadNextAsync().Result;
            IEnumerable<Account> accounts = currentResultSet.Resource;


            // Check if the operation returned any accounts
            if (!accounts.Any())
            {
                return new Result<Account>()
                {
                    Succeeded = false,
                    ResultType = ResultType.NotFound
                };
            }

            // The query returned an account, our result should be the only account in the list
            Account account = accounts.FirstOrDefault();
            return new Result<Account>()
            {
                Succeeded = true,
                Value = account
            };
        }

        public Result<Account> UpdateAccount(Account updatedAccount)
        {
            throw new System.NotImplementedException();
        }
    }
}
