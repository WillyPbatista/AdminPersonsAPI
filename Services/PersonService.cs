using AutoMapper;
using CRUD_Persons.Controllers;
using CRUD_Persons.Data;
using CRUD_Persons.DTOs;
using CRUD_Persons.Interfaces;
using CRUD_Persons.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Persons.Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonService> _logger;
        private readonly IMapper _mapper;
        public PersonService(ApplicationDbContext db, ILogger<PersonService> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;   
        }

        public async Task<CreatePersonDTO> CreatePersonAsync(CreatePersonDTO createdPersonDTO)
        {
            if (await _db.Persons.FirstOrDefaultAsync(x => x.Name.ToLower() == createdPersonDTO.Name.ToLower() &&
                                                           x.LastName.ToLower() == createdPersonDTO.LastName.ToLower()) != null)
            {
                _logger.LogError("existPerson", "This Person already exist");
                return null;
            }

            //Person model = new()
            //{
            //    Name = createdPersonDTO.Name,
            //    LastName = createdPersonDTO.LastName,
            //    Age = createdPersonDTO.Age,
            //    Occupation = createdPersonDTO.Occupation,
            //    Birthday = createdPersonDTO.Birthday
            //};

            _db.Persons.Add(_mapper.Map<Person>(createdPersonDTO));
            await _db.SaveChangesAsync();

            return createdPersonDTO;
        }

        public async Task DeletePersonAsync(int ID)
        {
            var deletedPerson = await _db.Persons.FirstOrDefaultAsync(x => x.ID == ID);
            if (deletedPerson != null)
            {
                _db.Persons.Remove(deletedPerson);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<PersonDTO> GetPersonByIDAsync(int ID)
        {
            var person = await _db.Persons.FirstOrDefaultAsync(x => x.ID == ID);
            if (person == null)
            {
                return null;
            }

            //PersonDTO findedPerson = new()
            //{
            //    ID = person.ID,
            //    Name = person.Name,
            //    LastName = person.LastName,
            //    Age = person.Age,
            //    Occupation = person.Occupation,
            //    Birthday = person.Birthday
            //};


            return _mapper.Map<PersonDTO>(person);
        }

        public async Task<List<PersonDTO>> GetPersonsAsync()
        {
            List<Person> allPersons = await _db.Persons.ToListAsync();
            //List<PersonDTO> allPersonsDTO = new();


            //foreach (Person person in allPersons)
            //{
            //    PersonDTO x = new PersonDTO
            //    {
            //        ID = person.ID,
            //        Name = person.Name,
            //        LastName = person.LastName,
            //        Age = person.Age,
            //        Birthday = person.Birthday,
            //        Occupation = person.Occupation
            //    };

            //    allPersonsDTO.Add(x);
            //}

            return _mapper.Map<List<PersonDTO>>(allPersons);
        }

        public async Task UpdatePartialPropertiesPersonAsync(int ID, JsonPatchDocument<UpdatePersonDTO> patchPersonDTO)
        {
            var updatedPerson = await _db.Persons.FirstOrDefaultAsync(x => x.ID == ID);
            if (updatedPerson != null)
            {
                UpdatePersonDTO patchedPerson = new()
                {
                    Name = updatedPerson.Name,
                    LastName = updatedPerson.LastName,
                    Age = updatedPerson.Age,
                    Occupation = updatedPerson.Occupation,
                    Birthday = updatedPerson.Birthday
                };

                patchPersonDTO.ApplyTo(patchedPerson);

                updatedPerson.Name = patchedPerson.Name;
                updatedPerson.LastName = patchedPerson.LastName;
                updatedPerson.Age = patchedPerson.Age;
                updatedPerson.Occupation = patchedPerson.Occupation;
                updatedPerson.Birthday = patchedPerson.Birthday;

                _db.Update(updatedPerson);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdatePersonAsync(int ID, UpdatePersonDTO personDTO)
        {
            var updatedPerson = await _db.Persons.FirstOrDefaultAsync(x => x.ID == ID);
            if (updatedPerson != null)
            {
                updatedPerson.Name = personDTO.Name;
                updatedPerson.LastName = personDTO.LastName;
                updatedPerson.Age = personDTO.Age;
                updatedPerson.Occupation = personDTO.Occupation;
                updatedPerson.Birthday = personDTO.Birthday;

                _db.Update(updatedPerson);
                await _db.SaveChangesAsync();
            }
        }
    }
}
