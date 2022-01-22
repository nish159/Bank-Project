namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using Models;
    using System.IO;
    using System.Linq;
    using System.Text.Json;

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
        public void CreateUser(User user)
        {
            List<User> users = GetAllUsers();
            // Checking if there is any account that has the same account number
            // as the account we want to create
            if (users.Any(i => i.UserName == user.UserName))
            {
                Console.WriteLine($"Unable to create username. Username {user.UserName} already exists");
                return;
            }

            users.Add(user);
            using (StreamWriter writer = new StreamWriter("../../../users.json"))
            {
                string usersJson = JsonSerializer.Serialize(users);
                writer.Write(usersJson);
            }
        }

        /// <summary>
        /// Updates the specified user data entity
        /// </summary>
        /// <param name="updatedUser">The user to be updated</param>
        public void UpdateUser(User updatedUser)
        {
            // Check if the account we want to update exists
            User existingUser = GetById(updatedUser.Id);
            if (existingUser == null)
            {
                Console.WriteLine($"Unable to update user. No user found with ID {updatedUser.Id} exists");
                // function executes until it meets first return statement 
                return;
            }

            User userByUserName = GetByUserName(updatedUser.UserName);
            if (userByUserName != null /*There is a user with the matching user name*/ &&
                userByUserName.Id != existingUser.Id /* The user with the mathing user name has a different id*/)
            {
                Console.WriteLine($"Unable to upate user. Duplicate user name");
                return;
            }

            // We have verified that the user exists - update the user
            List<User> users = GetAllUsers();
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
        }

        /// <summary>
        /// Updates the specified user data entity
        /// </summary>
        /// <param name="oldUserName">The user's user name before the update</param>
        /// <param name="updatedUser">The user to be updated</param>
        public void UpdateUser(string oldUserName, User updatedUser)
        {
            // Check if the account we want to update exists
            User existingUserName = GetByUserName(oldUserName);
            if (existingUserName == null)
            {
                Console.WriteLine($"Unable to update user. No user with that username {oldUserName} exists");
                // function executes until it meets first return statement 
                return;
            }

            // We have verified that the user exists - update the user
            List<User> users = GetAllUsers();
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
        }

        /// <summary>
        /// Deletes the specified user data entity
        /// </summary>
        /// <param name="deleteUser">The user to be deleted</param>
        public void DeleteUser(User deleteUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of all users in the system
        /// </summary>
        /// <returns>A list of all <see cref="User"/>s in the system</returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (StreamReader reader = new StreamReader("../../../users.json"))
            {
                string usersJson = reader.ReadToEnd();
                users = JsonSerializer.Deserialize<List<User>>(usersJson);
            }

            return users;
        }

        /// <summary>
        /// Gets a list of all users that have the given user name
        /// </summary>
        /// <param name="userName">Unique user identifier</param>
        /// <returns>A list of all <see cref="User"/>s that have the given user name</returns>
        public List<User> GetAllByUserName(string userName)
        {
            List<User> users = GetAllUsers();

            List<User> allUsers = users.Where(i => i.UserName == userName).ToList();
            return allUsers;
        }

        /// <summary>
        /// Gets a list of all users that have the given first name
        /// </summary>
        /// <param name="firstName">User's first name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given girst name</returns>
        public List<User> GetAllByFirstName(string firstName)
        {
            List<User> users = GetAllUsers();

            List<User> allFirst = users.Where(i => i.FirstName == firstName).ToList();
            return allFirst;
        }

        /// <summary>
        /// Gets a list of all users that have the given last name
        /// </summary>
        /// <param name="lastName">User's last name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given last name</returns>
        public List<User> GetAllByLastName(string lastName)
        {
            List<User> users = GetAllUsers();

            List<User> allLast = users.Where(i => i.LastName == lastName).ToList();
            return allLast;
        }

        /// <summary>
        /// Gets a list of all users that have the given first name and last name
        /// </summary>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given first name and last name</returns>
        public List<User> GetAllByName(string firstName, string lastName)
        {
            List<User> users = GetAllUsers();

            List<User> allUsers = users.Where(i => i.FirstName == firstName && i.LastName == lastName).ToList();
            return allUsers;
        }

        /// <summary>
        /// Gets the user with the given user name
        /// </summary>
        /// <param name="userName">Unique user identifier</param>
        /// <returns>The <see cref="User"/> with the given user name, or null if no user exists with that user name</returns>
        public User GetByUserName(string userName)
        {
            List<User> users = GetAllUsers();

            // user will either be the user with the matching username, or null if there is no account with that username
            User user = users.Where(i => i.UserName == userName).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Gets the user with the given user id
        /// </summary>
        /// <param name="id">Unique user identifier</param>
        /// <returns>The <see cref="User"/> with the given id, or null if no user exists with that id</returns>
        public User GetById(string ID)
        {
            List<User> users = GetAllUsers();

            User user = users.Where(i => i.Id == ID).FirstOrDefault();
            return user;
        }
    }
}
