using Entities.Models;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace Services
{
    public class PersonService : IPersonService
    {

        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }


        public PersonResponse AddPerson(PersonAddRequest? personRequest)
        {
            if (personRequest == null) throw new ArgumentNullException(nameof(personRequest));

            ValidationHelper.ModelValidation(personRequest);

            Person person = personRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            _persons.Add(person);

           PersonResponse personResponse = convertPersonToPersonResponse(person);

           return personResponse;
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null) return null;

            Person? person = _persons.FirstOrDefault(p => p.PersonID == personId);
            if (person == null) return null;

            PersonResponse personResponse = person.ToPersonResponse();
            return personResponse;
        }


        /// <summary>
        /// Used To Convert Person to Person Response
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Person Response</returns>
        private PersonResponse? convertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryId)?.CountryName;
            return personResponse;
        }
    }
}
