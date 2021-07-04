using System;
using System.ComponentModel.DataAnnotations;

namespace AlintaDomain
{
    public class Customer
    {
        public int? Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
