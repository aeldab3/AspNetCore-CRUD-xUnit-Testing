using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace Entities.Models
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");


            string countriesJson = File.ReadAllText("countries.json");

            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (var country in countries)
                modelBuilder.Entity<Country>().HasData(country);


            string personsJson = File.ReadAllText("persons.json");

            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (var person in persons)
                modelBuilder.Entity<Person>().HasData(person);
        }

        public IEnumerable<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryId", person.CountryId),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] " +
                "@PersonId, @PersonName, @Email, @DateOfBirth, " +
                "@Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters)
            ;
        }
    }
}
