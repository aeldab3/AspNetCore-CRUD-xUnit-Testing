using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace Entities.Models
{
    public class PersonsDbContext : DbContext
    {
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
    }
}
