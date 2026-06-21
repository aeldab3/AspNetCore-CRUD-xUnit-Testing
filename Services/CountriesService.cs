using Entities.Models;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    /// <summary>
    /// This class implements the ICountriesService interface and provides functionality for managing countries in the application.
    /// </summary>
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
        }


        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (countryAddRequest.CountryName == null)
                throw new ArgumentException("CountryName cannot be null", nameof(countryAddRequest.CountryName));

            var newCountry = _countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0;
            if (newCountry)
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            _countries.Add(country);
            return country.ToCountryResponse();
        }


        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(c => c.ToCountryResponse()).ToList();
        }


        public CountryResponse GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) return null;

            Country? country = _countries.FirstOrDefault(c => c.CountryID == countryID);

            if (country == null) return null;

            return country.ToCountryResponse();
        }
    }
}
