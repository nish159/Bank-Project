namespace DataAccess
{
    using System.Collections.Generic;
    using Models;
    using Bank;

    /// <summary>
    /// Defines the interface for interacting with the account data store.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user data entity
        /// </summary>
        /// <param name="user">The user to be created</param>
        Result<User> CreateUser(User user);

        /// <summary>
        /// Updates the specified user data entity
        /// </summary>
        /// <param name="updatedUser">The user to be updated</param>
        Result<User> UpdateUser(User updatedUser);

        /// <summary>
        /// Updates the specified user data entity
        /// </summary>
        /// <param name="oldUserName">The user's user name before the update</param>
        /// <param name="updatedUser">The user to be updated</param>
        Result<User> UpdateUser(string oldUserName, User updatedUser);

        /// <summary>
        /// Deletes the specified user data entity
        /// </summary>
        /// <param name="deleteUser">The user to be deleted</param>
        Result<User> DeleteUser(User deleteUser);

        /// <summary>
        /// Gets a list of all users in the system
        /// </summary>
        /// <returns>A list of all <see cref="User"/>s in the system</returns>
        Result<List<User>> GetAllUsers();

        /// <summary>
        /// Gets a list of all users that have the given user name
        /// </summary>
        /// <param name="userName">Unique user identifier</param>
        /// <returns>A list of all <see cref="User"/>s that have the given user name</returns>
        Result<List<User>> GetAllByUserName(string userName);

        /// <summary>
        /// Gets a list of all users that have the given first name
        /// </summary>
        /// <param name="firstName">User's first name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given girst name</returns>
        Result<List<User>> GetAllByFirstName(string firstName);

        /// <summary>
        /// Gets a list of all users that have the given last name
        /// </summary>
        /// <param name="lastName">User's last name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given last name</returns>
        Result<List<User>> GetAllByLastName(string lastName);

        /// <summary>
        /// Gets a list of all users that have the given first name and last name
        /// </summary>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <returns>A list of all <see cref="User"/>s that have the given first name and last name</returns>
        Result<List<User>> GetAllByName(string firstName, string lastName);

        /// <summary>
        /// Gets the user with the given user name
        /// </summary>
        /// <param name="userName">Unique user identifier</param>
        /// <returns>The <see cref="User"/> with the given user name, or null if no user exists with that user name</returns>
        Result<User> GetByUserName(string userName);

        /// <summary>
        /// Gets the user with the given user id
        /// </summary>
        /// <param name="id">Unique user identifier</param>
        /// <returns>The <see cref="User"/> with the given id, or null if no user exists with that id</returns>
        Result<User> GetById(string id);
    }
}

