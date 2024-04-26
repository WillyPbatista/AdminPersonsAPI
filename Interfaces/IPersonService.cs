using CRUD_Persons.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace CRUD_Persons.Interfaces
{
    public interface IPersonService
    {
        IEnumerable<PersonDTO> GetPersons();
        PersonDTO GetPersonByID(int ID);
        PersonDTO CreatePerson(PersonDTO personDTO);
        void DeletePerson(int ID);
        void UpdatePerson(int ID, PersonDTO personDTO);
        void UpdatePartialPropertiesPerson(int ID, JsonPatchDocument<PersonDTO> pacthPersonDTO);
    }
}
