using System.ComponentModel.DataAnnotations;

namespace CRUD_Persons.DTOs
{
    public class CreatePersonDTO
    {
       
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public DateTime Birthday { get; set; }
    }
}
