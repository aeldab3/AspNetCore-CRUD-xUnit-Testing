using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (countryAddRequest.CountryName == null)
                throw new ArgumentException("CountryName cannot be null", nameof(countryAddRequest.CountryName));

            var newCountry = await _db.Countries.CountAsync(c => c.CountryName == countryAddRequest.CountryName) > 0;
            if (newCountry)
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));

            Country country = countryAddRequest.ToCountry();
            country.CountryID = Guid.NewGuid();

            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }


        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _db.Countries.Select(c => c.ToCountryResponse()).ToListAsync();
        }


        public async Task<CountryResponse> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) return null;

            Country? country = await _db.Countries.FirstOrDefaultAsync(c => c.CountryID == countryID);

            if (country == null) return null;

            return country.ToCountryResponse();
        }
    }
}
