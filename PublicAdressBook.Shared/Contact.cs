using System;
using System.ComponentModel.DataAnnotations;

namespace PublicAdressBook.Shared
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name is long too long")]
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string MobilePhone { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }
    }
}