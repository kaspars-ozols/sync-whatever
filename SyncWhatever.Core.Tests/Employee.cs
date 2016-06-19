using System;

namespace SyncWhatever.Core.Tests
{
    public class Employee
    {
        public Employee(Guid id, string fullName, string email)
        {
            Id = id;
            FullName = fullName;
            Email = email;
        }


        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

    }
}