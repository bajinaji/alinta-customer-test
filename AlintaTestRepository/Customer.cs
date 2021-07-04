using System;
using System.ComponentModel.DataAnnotations;
using CustomDataAnnotations;

namespace AlintaDomain
{
    public class Customer
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [ValidDateOfBirthDataAnnotation]
        public DateTime? DateOfBirth { get; set; }
    }
}
