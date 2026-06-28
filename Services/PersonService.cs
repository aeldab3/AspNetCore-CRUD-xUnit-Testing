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

        private readonly PersonsDbContext _db;
        private readonly ICountriesService _countriesService;

        public PersonService(PersonsDbContext db, ICountriesService countriesService)
        {
            _db = db;
            _countriesService = countriesService;
        }


        public PersonResponse AddPerson(PersonAddRequest? personRequest)
        {
            if (personRequest == null) throw new ArgumentNullException(nameof(personRequest));

            ValidationHelper.ModelValidation(personRequest);

            Person person = personRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            _db.Persons.Add(person);
            _db.SaveChanges();

           PersonResponse personResponse = convertPersonToPersonResponse(person);

           return personResponse;
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _db.Persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingPersons;

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.PersonName) ?
                    temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;


                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryId):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }

        //public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons ,string sortBy, SortOrderOptions sortOrder)
        //{
        //    if (string.IsNullOrEmpty(sortBy)) return allPersons;
        //}


        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null) return null;

            Person? person = _db.Persons.FirstOrDefault(p => p.PersonID == personId);
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
