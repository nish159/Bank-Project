namespace Models
{
    using System;

    /// <summary>
    /// Defines a User object - the top-level representation of a user
    /// in the system.
    /// </summary>
    public class BankUser
    {
        /// <summary>
        /// Unique document identifier.
        /// The document identifier is used for document-based repositories and is the string
        /// representation of the user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique user identifier used when authenticating the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's password used for authentication
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Creates a new istance
        /// </summary>
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
