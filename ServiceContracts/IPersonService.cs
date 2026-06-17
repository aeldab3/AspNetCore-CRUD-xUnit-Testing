using ServiceContracts.DTO;
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
    }
}
