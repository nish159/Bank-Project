namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Models;
    using System.IO;
    using System.Linq;
    using System.Text.Json;

    public interface IData
    {
        // function to get a list of users
        List<User> GetAllUsers();
        // function to get a list of all usernames 
        List<User> GetAllUserNames(string userName);

        // function to get a list of all first names
        List<User> GetAllFirstNames(string firstName);

        // function to get a list of all last names
        List<User> GetAllLastNames(string lastName);

        // function to get a list by first and last names 
        List<User> GetFirstLast(string firstName, string lastName);

        // function to get a single username 
        User GetByUserName(string userName);

        // function to create a user 
        void CreatAUser(User user);

        // function to update the user information 
        void UpdateUser(User updatedUser);

        // function to update the user information 
        void UpdateUser(string oldUserName, User updatedUser);

        // function to delete a user
        void DeleteUser(User deleteUser);

        // function to get by guid
        User GetById(string ID);
    }

    public class Data : IData
    {
        // you need to set the parameteres for  creating the user
        public void CreatAUser(User user)
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

        public void DeleteUser(User deleteUser)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllFirstNames(string firstName)
        {
            List<User> users = GetAllUsers();

            List<User> allFirst = users.Where(i => i.FirstName == firstName).ToList();
            return allFirst;
        }

        public List<User> GetAllLastNames(string lastName)
        {
            List<User> users = GetAllUsers();

            List<User> allLast = users.Where(i => i.LastName == lastName).ToList();
            return allLast;
        }

        public List<User> GetAllUserNames(string userName)
        {
            List<User> users = GetAllUsers();

            List<User> allUsers = users.Where(i => i.UserName == userName).ToList();
            return allUsers;
        }

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

        public User GetById(string ID)
        {
            List<User> users = GetAllUsers();

            User user = users.Where(i => i.Id == ID).FirstOrDefault();
            return user;
        }

        public User GetByUserName(string userName)
        {
            List<User> users = GetAllUsers();

            // user will either be the user with the matching username, or null if there is no account with that username
            User user = users.Where(i => i.UserName == userName).FirstOrDefault();
            return user;
        }

        public List<User> GetFirstLast(string firstName, string lastName)
        {
            List<User> users = GetAllUsers();

            List<User> allUsers = users.Where(i => i.FirstName == firstName && i.LastName == lastName).ToList();
            return allUsers;
        }

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
            if(userByUserName != null /*There is a user with the matching user name*/ && 
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
    }
}

namespace Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
