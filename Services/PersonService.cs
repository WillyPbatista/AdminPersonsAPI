using CRUD_Persons.Controllers;
using CRUD_Persons.Data;
using CRUD_Persons.DTOs;
using CRUD_Persons.Interfaces;
using CRUD_Persons.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace CRUD_Persons.Services
{
    public class PersonService : IPersonService
       
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonService> _logger;
        public PersonService(ApplicationDbContext db, ILogger<PersonService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public PersonDTO CreatePerson(PersonDTO personDTO)
        {
            if (_db.Persons.FirstOrDefault(x => x.Name.ToLower() == personDTO.Name.ToLower()) != null
                &&
               _db.Persons.FirstOrDefault(x => x.LastName.ToLower() == personDTO.LastName.ToLower()) != null)
            {
                _logger.LogError("existPerson", "This Person already exist");
                return null;
            }
            Person model = new()
            {
                
                Name = personDTO.Name,
                LastName = personDTO.LastName,
                Age = personDTO.Age,
                Occupation = personDTO.Occupation,
                Birthday = personDTO.Birthday
            };
            _db.Persons.Add(model);
            _db.SaveChanges();

            return personDTO;
        }

        public void DeletePerson(int ID)
        {
            var deletedPerson = _db.Persons.FirstOrDefault(x => x.ID == ID);
            if(deletedPerson != null)
            _db.Persons.Remove(deletedPerson);
            _db.SaveChanges();
        }

        public PersonDTO GetPersonByID(int ID)
        {
            var person = _db.Persons.FirstOrDefault(x => x.ID == ID);
            PersonDTO findedPerson = new()
            {
                ID = person.ID,
                Name = person.Name,
                LastName = person.LastName,
                Age = person.Age,
                Occupation = person.Occupation,
                Birthday = person.Birthday
            };
            return findedPerson;

        }

        public IEnumerable<PersonDTO> GetPersons()
        {
           
            List<Person> AllPersons = _db.Persons.ToList();
            List<PersonDTO> AllPersonsDTO = new();

            foreach (Person person  in AllPersons)
            {
                PersonDTO x = new PersonDTO();
                x.ID = person.ID;
                x.Name = person.Name;
                x.LastName = person.LastName;
                x.Age = person.Age;
                x.Birthday = person.Birthday;
                x.Occupation = person.Occupation;

                AllPersonsDTO.Add(x);    
            }
            return AllPersonsDTO;
        }

        public void UpdatePartialPropertiesPerson(int ID, JsonPatchDocument<PersonDTO> pacthPersonDTO)
        {
            var updatedPerson = _db.Persons.FirstOrDefault(x => x.ID == ID);
            if (updatedPerson != null)
            {
                PersonDTO patchedPerson = new()
                {
                    
                    Name = updatedPerson.Name,
                    LastName = updatedPerson.LastName,
                    Age = updatedPerson.Age,
                    Occupation = updatedPerson.Occupation,
                    Birthday = updatedPerson.Birthday
                };
                Person updatedPersonF = new()
                {
                    
                    Name = patchedPerson.Name,
                    LastName = patchedPerson.LastName,
                    Age = patchedPerson.Age,
                    Occupation = patchedPerson.Occupation,
                    Birthday = patchedPerson.Birthday
                };
                pacthPersonDTO.ApplyTo(patchedPerson);
                _db.Update(updatedPersonF);
                _db.SaveChanges();
            }
        }

        public void UpdatePerson(int ID, PersonDTO personDTO)
        {
            var updatedPerson = _db.Persons.FirstOrDefault(x => x.ID == ID);
            if(updatedPerson != null)
            {
                updatedPerson.Name = personDTO.Name;
                updatedPerson.LastName = personDTO.LastName;
                updatedPerson.Age = personDTO.Age;
            }
            _db.Update(updatedPerson);
            _db.SaveChanges();


        }
    }
}
