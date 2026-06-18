using Entities.Models;
using ServiceContracts;
using ServiceContracts.DTO;


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
            if (string.IsNullOrWhiteSpace(personRequest.PersonName)) throw new ArgumentException(nameof(personRequest.PersonName));

            Person person = personRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            _persons.Add(person);

           PersonResponse personResponse = convertPersonToPersonResponse(person);

           return personResponse;
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private PersonResponse convertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryId)?.CountryName;
            return personResponse;
        }
    }
}
