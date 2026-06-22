using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }


        #region Add Person
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

        #endregion

        #region Get Person By Id

        // When you get Person By Null Id
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

        // When you get Person by person ID
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

        #endregion


        #region Get All Persons

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            // Act
            List<PersonResponse> AllPersons = _personService.GetAllPersons();

            // Assert
            Assert.Empty(AllPersons);
        }


        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            // Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "UAE"
            };
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            List<PersonResponse> personResponses_list_from_added = new List<PersonResponse>();

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Mohammed",
                Address = "Tanta",
                Email = "mo@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = countryResponse1.CountryID,
                DateOfBirth = DateTime.Parse("1957-12-17"),
                ReceiveNewsLetters = true,
            };

            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Ali",
                Address = "Tanta",
                Email = "ali@gmail.com",
                Gender = GenderOptions.Male,
                CountryId = countryResponse1.CountryID,
                DateOfBirth = DateTime.Parse("1993-09-01"),
                ReceiveNewsLetters = true,
            };

            List<PersonAddRequest> addAllPersons = new List<PersonAddRequest>()
            {
                personAddRequest1, personAddRequest2
            };

            foreach (PersonAddRequest personAddRequest in addAllPersons)
            {
                PersonResponse allPersonsAdded = _personService.AddPerson(personAddRequest);
                personResponses_list_from_added.Add(allPersonsAdded);
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse expectedPerson in personResponses_list_from_added)
            {
                _testOutputHelper.WriteLine($"{expectedPerson.ToString()}");
            }

            // Act
            List<PersonResponse> getAllPersons = _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in getAllPersons)
            {
                _testOutputHelper.WriteLine($"{person.ToString()}");
            }

            // Assert
            foreach (PersonResponse person in personResponses_list_from_added)
            {
                Assert.Contains(person, getAllPersons);
            }
        }

        #endregion

    }
}
