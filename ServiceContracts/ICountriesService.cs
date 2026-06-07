using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents the service contract for managing countries in the application. This interface defines the operations that can be performed related to countries, such as retrieving country information, adding new countries, updating existing countries, and deleting countries.
    /// </summary>
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();
    }
}
