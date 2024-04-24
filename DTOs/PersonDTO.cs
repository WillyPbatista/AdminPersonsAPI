using System.ComponentModel.DataAnnotations;

namespace CRUD_Persons.DTOs
{
    public class PersonDTO
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
