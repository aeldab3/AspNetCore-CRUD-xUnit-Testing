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
            _countriesService = new CountriesService();
        }


        #region Add Country Tests

        // When Country Add Request is Null
        [Fact]
        public void AddCountry_NullCountry()
        {
            // Arrange 
            CountryAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When Country Name is Null
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            // Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When the country name is duplicate
        [Fact]
        public void AddCountry_DuplicateCountryName()
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
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        // when you supply proper country name, then it should be added to the country list
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange 
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Saudi Arabia"
            };

            // Act
            CountryResponse response = _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries =  _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryID != Guid.Empty);

            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion


        #region get all countries tests

        // when there are no countries in the list, then it should return empty list
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> countryResponses = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(countryResponses);
        }

        // when there are some countries in the list, then it should return list of countries
        [Fact]
        public void GetAllCountries_AddFewCountries()
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
                countryResponses.Add(_countriesService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualCountrieResponsesList =  _countriesService.GetAllCountries();

            // Read each country response from the list of country responses and check whether it is present in the actual country responses list or not
            // Assert
            foreach (CountryResponse countryResponse in countryResponses)
            {
                Assert.Contains(countryResponse, actualCountrieResponsesList);
            }
        }
        #endregion
    }
}
