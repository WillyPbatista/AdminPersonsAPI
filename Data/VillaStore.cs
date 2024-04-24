using CRUD_Persons.DTOs;

namespace CRUD_Persons.Data
{
    public static class VillaStore
    {
        public static List<PersonDTO> PersonList = new List<PersonDTO> {

            new PersonDTO{ID = 128404, Name = "Elsa", LastName = "Pato", Age = 17 },
            new PersonDTO{ID = 456231, Name = "Elver", LastName = "Galarga", Age = 19 }

        };

    };
}
