using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;

        public PersonsServiceTest()
        {
            _personService = new PersonService();
        }


        // When Supply Null Value
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        // When Supply Null Value for personName
        [Fact]
        public void AddPerson_NullPersonName()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }


        // When Supply proper person details, and add new person to persons list
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Ahmed",
                Email = "a@gmail.com",
                Address = "Kafr El-Zayay",
                DateOfBirth = DateTime.Parse("1999-01-18"),
                Gender = GenderOptions.Male,
                CountryId = Guid.NewGuid(),
                ReceiveNewsLetters = true
            };

            // Act
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            List<PersonResponse> personsList =  _personService.GetAllPersons();

            // Assert
            Assert.True(personResponse.PersonID != Guid.Empty);
            Assert.Contains(personResponse, personsList);

        }

    }
}
