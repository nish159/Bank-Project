namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using Models;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using Bank;

    /// <summary>
    /// Implements the <see cref="IUserRepository"/> interface for
    /// interacting with the user file system data store
    /// </summary>
    public class UserFileSystemRepository : IUserRepository
    {
        /// <summary>
        /// Creates a new user data entity
        /// </summary>
        /// <param name="user">The user to be created</param>
        public Result<User> CreateUser(User user)
        {
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;
            // Checking if there is any account that has the same account number
            // as the account we want to create
            if (users.Any(i => i.UserName == user.UserName))
            {
                return new Result<User>()
                {
                    Succeeded = false,
                    Message = $"Unable to create username. Username {user.UserName} already exists",
                    ResultType = ResultType.Duplicate
                };
            }

            users.Add(user);
            using (StreamWriter writer = new StreamWriter("../../../users.json"))
            {
                string usersJson = JsonSerializer.Serialize(users);
                writer.Write(usersJson);
            }

            return new Result<User>()
            {
                Succeeded = true,
                Value = user
            };
        }

        /// <summary>
        /// Updates the specified user data entity
        /// </summary>
        /// <param name="updatedUser">The user to be updated</param>
        public Result<User> UpdateUser(User updatedUser)
        {
            // Check if the account we want to update exists
            Result<User> getAllUserNamesResult = GetById(updatedUser.Id);
            if (getAllUserNamesResult.Succeeded == false)
            {
                return getAllUserNamesResult;
            }

            Result<User> existingUser = GetById(updatedUser.Id);
            if (existingUser.Succeeded == false)
            {
                return new Result<User>()
                {
                    Succeeded = false,
                    Message = $"Unable to update user. No user found with ID {updatedUser.Id} exists",
                    ResultType = ResultType.NotFound
                };
            }

            Result<User> getByUserNameResult = GetByUserName(updatedUser.UserName);
            if (getByUserNameResult.Succeeded == true /*There is a user with the matching user name*/ &&
                // getByUserNameResult.Value is the existing user with the given user name
                getByUserNameResult.Value.Id != existingUser.Value.Id /* The user with the mathing user name has a different id*/)
            {
                return new Result<User>()
                {
                    Succeeded = false,
                    Message = $"Unable to upate user. Duplicate user name",
                    ResultType = ResultType.Duplicate
                };
            }

            // We have verified that the user exists - update the user
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;
            foreach (User user in users)
            {
                if (user.Id == updatedUser.Id)
                {
                    user.FirstName = updatedUser.FirstName;
                    user.LastName = updatedUser.LastName;
                    user.UserName = updatedUser.UserName;
                }
            }
            using (StreamWriter writer = new StreamWriter("../../../users.json"))
            {
                string usersJson = JsonSerializer.Serialize(users);
                writer.Write(usersJson);
            }
            
            // Operation was successful
            return new Result<User>()
            {
                Succeeded = true,
                Value = updatedUser
            };
        }

        /// <summary>
        /// Updates the specified user data entity
        /// </summary>
        /// <param name="oldUserName">The user's user name before the update</param>
        /// <param name="updatedUser">The user to be updated</param>
        public Result<User> UpdateUser(string oldUserName, User updatedUser)
        {
            // Check if the account we want to update exists
            Result<User> existingUserName = GetByUserName(oldUserName);
            if (existingUserName.Succeeded == false)
            {
                return new Result<User>()
                {
                    Succeeded = false,
                    Message = $"Unable to update user. No user with that username {oldUserName} exists",
                    ResultType = ResultType.NotFound
                };
            }

            // We have verified that the user exists - update the user
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;
            foreach (User user in users)
            {
                if (user.UserName == oldUserName)
                {
                    user.UserName = updatedUser.UserName;
                    user.FirstName = updatedUser.FirstName;
                    user.LastName = updatedUser.LastName;
                }
            }
            using (StreamWriter writer = new StreamWriter("../../../users.json"))
            {
                string usersJson = JsonSerializer.Serialize(users);
                writer.Write(usersJson);

            }

            // Operation was successful
            return new Result<User>()
            {
                Succeeded = true,
                Value = updatedUser
            };
        }

        /// <summary>
        /// Deletes the specified user data entity
        /// </summary>
        /// <param name="deletedUser">The user to be deleted</param>
        public Result<User> DeleteUser(User deletedUser)
        {
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> allUsers = getAllUsersResult.Value;

            //allUsers.Remove(deletedUser);
            allUsers = allUsers.Where(i => i.UserName != deletedUser.UserName).ToList();
            using (StreamWriter writer = new StreamWriter("../../../users.json"))
            {
                string userJson = JsonSerializer.Serialize(allUsers);
                writer.Write(userJson);
            }

            Console.WriteLine($"User {deletedUser.UserName} has been deleted");
            // Operation was successful
            return new Result<User>()
            {
                Succeeded = true,
                Value = deletedUser
            };
        }

        /// <summary>
        /// Gets a list of all users in the system
        /// </summary>
        /// <returns>A list of all <see cref="User"/>s in the system</returns>
        public Result<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (StreamReader reader = new StreamReader("../../../users.json"))
            {
                string usersJson = reader.ReadToEnd();
                users = JsonSerializer.Deserialize<List<User>>(usersJson);
            }
            Result<List<User>> result = new Result<List<User>>()
            {
                Succeeded = true,
                Value = users
            };
            return result;
        }

        /// <summary>
        /// Gets a list of all users that have the given user name
        /// </summary>
        /// <param name="userName">Unique user identifier</param>
        /// <returns>A list of all <see cref="User"/>s that have the given user name</returns>
        public Result<List<User>> GetAllByUserName(string userName)
        {
            // Get all accounts in the system (json file)
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;

            // Filter the list to only have accounts for the given user name
            List<User> allUsers = users.Where(i => i.UserName == userName).ToList();
            Result<List<User>> result = new Result<List<User>>()
            {
                Succeeded = true,
                Value = allUsers
            };
            return result;
        }

        /// <summary>
        /// Gets a list of all users that have the given first name
        /// </summary>
        /// <param name="firstName">User's first name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given girst name</returns>
        public Result<List<User>> GetAllByFirstName(string firstName)
        {
            // Get all accounts in the system (json file)
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;

            // Filter the list to only have accounts for the given first name
            List<User> allFirst = users.Where(i => i.FirstName == firstName).ToList();
            Result<List<User>> result = new Result<List<User>>()
            {
                Succeeded = true,
                Value = allFirst
            };
            return result;
        }

        /// <summary>
        /// Gets a list of all users that have the given last name
        /// </summary>
        /// <param name="lastName">User's last name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given last name</returns>
        public Result<List<User>> GetAllByLastName(string lastName)
        {
            // Get all accounts in the system (json file)
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;

            // Filter the list to only have accounts for the given last name
            List<User> allLast = users.Where(i => i.LastName == lastName).ToList();
            Result<List<User>> result = new Result<List<User>>()
            {
                Succeeded = true,
                Value = allLast
            };
            return result;
        }

        /// <summary>
        /// Gets a list of all users that have the given first name and last name
        /// </summary>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given first name and last name</returns>
        public Result<List<User>> GetAllByName(string firstName, string lastName)
        {
            // Get all accounts in the system (json file)
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;

            // Filter the list to only have accounts for the given all names
            List<User> allUsers = users.Where(i => i.FirstName == firstName && i.LastName == lastName).ToList();
            Result<List<User>> result = new Result<List<User>>()
            {
                Succeeded = true,
                Value = allUsers
            };
            return result;
        }

        /// <summary>
        /// Gets the user with the given user name
        /// </summary>
        /// <param name="userName">Unique user identifier</param>
        /// <returns>The <see cref="User"/> with the given user name, or null if no user exists with that user name</returns>
        public Result<User> GetByUserName(string userName)
        {
            // Get all accounts in the system (json file)
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;

            // user will either be the user with the matching username, or null if there is no account with that username
            User user = users.Where(i => i.UserName == userName).FirstOrDefault();
            return new Result<User>
            {
                Succeeded = true,
                Value = user
            };
        }

        /// <summary>
        /// Gets the user with the given user id
        /// </summary>
        /// <param name="id">Unique user identifier</param>
        /// <returns>The <see cref="User"/> with the given id, or null if no user exists with that id</returns>
        public Result<User> GetById(string id)
        {
            Result<List<User>> getAllUsersResult = GetAllUsers();
            List<User> users = getAllUsersResult.Value;

            User user = users.Where(i => i.Id == id).FirstOrDefault();
            return new Result<User>
            {
                Succeeded = true,
                Value = user
            };
        }
    }
}
