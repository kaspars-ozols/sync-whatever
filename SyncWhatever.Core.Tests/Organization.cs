using System;
using System.Collections.Generic;

namespace SyncWhatever.Core.Tests
{
    [Serializable]
    public class Organization
    {
        public Organization(Guid id, string registrationNumber, string name)
        {
            Id = id;
            RegistrationNumber = registrationNumber;
            Name = name;
        }
        public Guid Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}