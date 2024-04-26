using CRUD_Persons.Data;
using CRUD_Persons.DTOs;
using CRUD_Persons.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace CRUD_Persons.Services
{
    public class PersonService : IPersonService
    {
        public PersonDTO CreatePerson(PersonDTO personDTO)
        {
            personDTO.ID = VillaStore.PersonList.OrderByDescending(x => x.ID).FirstOrDefault().ID + 1;

            VillaStore.PersonList.Add(personDTO);

            return personDTO;
        }

        public void DeletePerson(int ID)
        {
            var deletedPerson = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);
            if(deletedPerson != null)
            VillaStore.PersonList.Remove(deletedPerson);
        }

        public PersonDTO GetPersonByID(int ID)
        {
            var person = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);
            return person;

        }

        public IEnumerable<PersonDTO> GetPersons()
        {
            return VillaStore.PersonList;
        }

        public void UpdatePartialPropertiesPerson(int ID, JsonPatchDocument<PersonDTO> pacthPersonDTO)
        {
            var updatedPerson = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);
            if (updatedPerson != null)
            {
                pacthPersonDTO.ApplyTo(updatedPerson);
            }
        }

        public void UpdatePerson(int ID, PersonDTO personDTO)
        {
            var updatedPerson = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);
            if(updatedPerson != null)
            {
                updatedPerson.Name = personDTO.Name;
                updatedPerson.LastName = personDTO.LastName;
                updatedPerson.Age = personDTO.Age;
            }

           
        }
    }
}
