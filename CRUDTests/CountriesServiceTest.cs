using Entities.Models;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDTests
{
    /// <summary>
    /// This class contains unit tests for the CountriesService class, which implements the ICountriesService interface.
    /// </summary>
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }


        #region Add Country Tests

        // When Country Add Request is Null
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            // Arrange 
            CountryAddRequest? request = null;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request);
            });
        }

        // When Country Name is Null
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request);
            });
        }

        // When the country name is duplicate
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            // Arrange 
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                // Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        // when you supply proper country name, then it should be added to the country list
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Saudi Arabia"
            };

            // Act
            CountryResponse response = await _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryID != Guid.Empty);

            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion


        #region Get all countries Tests

        // when there are no countries in the list, then it should return empty list
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> countryResponses = await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countryResponses);
        }

        // when there are some countries in the list, then it should return list of countries
        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            // Arrange
            List<CountryResponse> countryResponses = new List<CountryResponse>();
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Egypt" },
                new CountryAddRequest() { CountryName = "Saudi Arabia" }
            };

            // Act
            foreach (CountryAddRequest countryAddRequest in country_request_list)
            {
                countryResponses.Add(await _countriesService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualCountrieResponsesList = await _countriesService.GetAllCountries();

            // Read each country response from the list of country responses and check whether it is present in the actual country responses list or not
            // Assert
            foreach (CountryResponse countryResponse in countryResponses)
            {
                Assert.Contains(countryResponse, actualCountrieResponsesList);
            }
        }
        #endregion

        #region Get Country By CountryID Tests

        // when you supply null country ID, then it should return null
        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            // Arrange
            Guid? countryId = null;

            // Act
            CountryResponse? countryResponse = await _countriesService.GetCountryByCountryID(countryId);

            // Assery
            Assert.Null(countryResponse);
        }


        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Egypt"
            };
            CountryResponse countryResponse = await _countriesService.AddCountry(request);

            // Act
            CountryResponse? actualCountryResponse = await _countriesService.GetCountryByCountryID(countryResponse.CountryID);

            // Assert
            Assert.Equal(countryResponse, actualCountryResponse);
        }

        #endregion
    }
}
