using System;
using System.ComponentModel.DataAnnotations;


namespace Entities.Models

{
    /// <summary>
    /// Domanin model for Country entity
    /// </summary>
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }

        [StringLength(80)]
        public string? CountryName { get; set; }
        public virtual ICollection<Person>? Persons { get; set; } 
    }
}
