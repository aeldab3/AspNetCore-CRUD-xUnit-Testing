using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
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
            List<PersonResponse> personsList = _personService.GetAllPersons();

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

        #region Filterd Persons

        // If The search text is Empty
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
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
            List<PersonResponse> personsListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personsListFromSearch)
            {
                _testOutputHelper.WriteLine($"{person.ToString()}");
            }

            // Assert
            foreach (PersonResponse person in personResponses_list_from_added)
            {
                Assert.Contains(person, personsListFromSearch);
            }
        }


        // If The search text with person name
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
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
            List<PersonResponse> personsListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "ed");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personsListFromSearch)
            {
                _testOutputHelper.WriteLine($"{person.ToString()}");
            }

            // Assert
            foreach (PersonResponse person in personResponses_list_from_added)
            {
                if (person.PersonName != null)
                {
                    if (person.PersonName.Contains("ed", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person, personsListFromSearch);
                    }
                }
            }
        }

        #endregion


        #region GetSortedPersons

        // When sort based on person name in DESC
        [Fact]
        public void GetSortedPersons_SortByPersonName()
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
                CountryId = countryResponse2.CountryID,
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

            List<PersonResponse> allPersons = _personService.GetAllPersons();

            // Act
            List<PersonResponse> personsListFromSort = _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personsListFromSort)
            {
                _testOutputHelper.WriteLine($"{person.ToString()}");
            }

            personResponses_list_from_added = personResponses_list_from_added.OrderByDescending(p => p.PersonName).ToList();

            // Assert
            for (int i = 0; i < personResponses_list_from_added.Count; i++)
            {
                Assert.Equal(personResponses_list_from_added[i], personsListFromSort[i]);
            }
        }

        #endregion


        #region UpdatePerson

        // When we supply Null as PersonUpdateRequest, It should throw ArgumentNullException.
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }


        // When we supply Invalid Person ID, It should throw ArgumentException.
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonID = Guid.NewGuid()
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }


        // When Person Name is null, It should throw ArgumentException.
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Lara",
                CountryId = countryResponse.CountryID,
                Email = "Lara@gmail.com",
                Address = "Address",
                Gender = GenderOptions.Female
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null; 

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }


        // When valid updated details are supplied, the person's information should be updated successfully
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "UK"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Lara",
                CountryId = countryResponse.CountryID,
                Address = "Kafer El_Zayat",
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2018-03-06"),
                Email = "lara@outlook.com",
                ReceiveNewsLetters = false
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest? personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Lolo";
            personUpdateRequest.Email = "lara@gmail.com";

            // Act
            PersonResponse personResponsFromUpdated =  _personService.UpdatePerson(personUpdateRequest);

            PersonResponse? perosnResonseFromGet = _personService.GetPersonByPersonId(personResponsFromUpdated.PersonID);

            // Assert
            Assert.Equal(perosnResonseFromGet, personResponsFromUpdated);
        }

        #endregion


        #region DeletePerson

        // If you Supply an Valid Person ID, It should return True
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Palastine"
            };

            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Abo Obaida",
                Address = "Jbalia",
                Email = "AboObaida@hamas.com",
                CountryId = countryResponse.CountryID,
                DateOfBirth = Convert.ToDateTime("2026-07-11"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            // Act
            bool isDeleted = _personService.DeletePerson(personResponse.PersonID);

            // Assert
            Assert.True(isDeleted);
        }


        // If you Supply an InValid Person ID, It should return True
        [Fact]
        public void DeletePerson_InValidPersonID()
        {
            // Act
            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

            // Assert
            Assert.False(isDeleted);
        }
        #endregion

    }
}
