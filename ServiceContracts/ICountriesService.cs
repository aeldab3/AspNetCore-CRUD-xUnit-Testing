using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents the service contract for managing countries in the application. This interface defines the operations that can be performed related to countries, such as retrieving country information, adding new countries, updating existing countries, and deleting countries.
    /// </summary>
    public interface ICountriesService
    {

        /// <summary>
        /// This method adds a new country to the list of countries.
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>CountryResponse object representing the newly added country.</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);


        /// <summary>
        /// This method retrieves a list of all countries.
        /// </summary>
        /// <returns>List of CountryResponse objects representing each country in the list.</returns>
        Task<List<CountryResponse>> GetAllCountries();


        /// <summary>
        /// This method retrieves a country based on the provided country ID.
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>It returns a CountryResponse object representing the country with the specified ID.</returns>
        Task<CountryResponse> GetCountryByCountryID(Guid? countryID);
    }
}
