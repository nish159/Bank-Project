namespace Models
{
    using System;

    public class Account
    {
        // each one should be unique
        public string Id { get; set; }
        public string UserName { get; set; }
        public int Number { get; set; }
        public decimal Amount { get; set; }
        public string Pin { get; set; }
        public Account()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
