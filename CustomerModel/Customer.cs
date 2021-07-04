using System;
using System.ComponentModel.DataAnnotations;

namespace AlintaTestModels
{
    public class Customer
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
