using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;

namespace ServiceContracts
{
    public interface IPersonService
    {
        /// <summary>
        /// Adds a new person to the data store.
        /// </summary>
        /// <param name="personRequest"></param>
        /// <returns>The added person as a PersonResponse object.</returns>
        PersonResponse AddPerson(PersonAddRequest? personRequest);


        /// <summary>
        /// Retrieves all persons from the data store.
        /// </summary>
        /// <returns>A list of all persons as PersonResponse objects.</returns>
        List<PersonResponse> GetAllPersons();


        /// <summary>
        /// Get Person By Person Id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>Person By Person Id <returns>
        PersonResponse? GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// Used this to Get Persons By spacific Filterd.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchString"></param>
        /// <returns>All matching persons based on the given search field and search string</returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);


        /// <summary>
        /// Used this to Sort List of Persons By Asccinding and Descinding.
        /// </summary>
        /// <param name="allPersons"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns>Sorted persons as PersonseResponse List</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);


        /// <summary>
        /// Used to Update Person
        /// </summary>
        /// <param name="personUpdateRequest"></param>
        /// <returns></returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    }
}
