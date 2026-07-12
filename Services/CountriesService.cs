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
        private readonly PersonsDbContext _db;
        public CountriesService(PersonsDbContext db)
        {
            _db = db;
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (countryAddRequest.CountryName == null)
                throw new ArgumentException("CountryName cannot be null", nameof(countryAddRequest.CountryName));

            var newCountry = _db.Countries.Count(c => c.CountryName == countryAddRequest.CountryName) > 0;
            if (newCountry)
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }


        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(c => c.ToCountryResponse()).ToList();
        }


        public CountryResponse GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) return null;

            Country? country = _db.Countries.FirstOrDefault(c => c.CountryID == countryID);

            if (country == null) return null;

            return country.ToCountryResponse();
        }
    }
}
