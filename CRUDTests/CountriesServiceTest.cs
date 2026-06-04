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

            // Assert
            Assert.True(response.CountryID != Guid.Empty);
        }
    }
}
