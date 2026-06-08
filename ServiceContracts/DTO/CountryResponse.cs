using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for representing a country response.
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        // Compare two CountryResponse objects based on their properties.
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(CountryResponse)) 
                return false;

            CountryResponse countryResponse = (CountryResponse)obj;

            return this.CountryID == countryResponse.CountryID && this.CountryName == countryResponse.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    // Extension method to convert a Country object to a CountryResponse object.
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
