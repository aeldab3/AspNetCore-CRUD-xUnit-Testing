using Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return PersonID == person.PersonID &&
                   PersonName == person.PersonName &&
                   Email == person.Email &&
                   DateOfBirth == person.DateOfBirth &&
                   Gender == person.Gender &&
                   CountryId == person.CountryId &&
                   Address == person.Address &&
                   ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Address: {Address}, Country Id: {CountryId}, Date Of Birth: {DateOfBirth}, Gender: {Gender}, Receive News Letters {ReceiveNewsLetters}";
        }
    }

    public static class PersonExtensions
    {
        /// <summary>
        /// Converts a Person entity to a PersonResponse DTO.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>The PersonResponse object with the Age property calculated based on the DateOfBirth of the Person entity.</returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = person.DateOfBirth.HasValue ? (DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25 : (double?)null

            };
        }
    }
}
