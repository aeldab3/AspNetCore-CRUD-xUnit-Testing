using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// Domain model for Person entity
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)]
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }

        [StringLength(80)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
    }
}
