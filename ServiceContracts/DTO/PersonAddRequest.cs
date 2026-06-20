using Entities.Models;
using ServiceContracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {

        [Required (ErrorMessage = "The Name is Required")]
        public string? PersonName { get; set; }

        [Required (ErrorMessage = "The Email is Required")]
        [EmailAddress (ErrorMessage = "You should add a vlid Email")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }


        /// <summary>
        /// Converts the current PersonAddRequest object to a Person object.
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
