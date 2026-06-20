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
        private readonly ICountriesService _countriesService;

        public PersonsServiceTest()
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
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


        // When
        [Fact]
        public void GetPersonByPersonId_NullPersonID()
        {
            // Arrange
            Guid? personId = null;

            // Act
            PersonResponse? personResponse = _personService.GetPersonByPersonId(personId);

            // Assert
            Assert.Null(personResponse);
        }

        // When
        [Fact]
        public void GetPersonByPersonID_WithPersonId() 
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            // Act
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Ahmed",
                Email = "a@gmail.com",
                Gender = GenderOptions.Male,
                Address = "Kz",
                DateOfBirth = DateTime.Parse("1999-01-18"),
                ReceiveNewsLetters = false,
                CountryId = countryResponse.CountryID
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonResponse? getPersonByPersonId = _personService.GetPersonByPersonId(personResponse.PersonID);

            // Assert
            Assert.Equal(personResponse, getPersonByPersonId);

        }
    }
}
