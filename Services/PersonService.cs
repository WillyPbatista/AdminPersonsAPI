using AutoMapper;
using CRUD_Persons.DTOs;
using CRUD_Persons.Interfaces;
using CRUD_Persons.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Persons.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;
        private readonly ILogger<PersonService> _logger;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository repository, ILogger<PersonService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CreatePersonDTO> CreatePersonAsync(CreatePersonDTO createdPersonDTO)
        {
            if (await _repository.FindByNameAsync(createdPersonDTO.Name, createdPersonDTO.LastName) != null)
            {
                _logger.LogError("existPerson", "This Person already exist");
                return null;
            }

            var person = _mapper.Map<Person>(createdPersonDTO);
            await _repository.AddAsync(person);

            return createdPersonDTO;
        }

        public async Task DeletePersonAsync(int ID)
        {
            await _repository.DeleteAsync(ID);
        }

        public async Task<PersonDTO> GetPersonByIDAsync(int ID)
        {
            var person = await _repository.GetByIdAsync(ID);
            if (person == null)
            {
                return null;
            }

            return _mapper.Map<PersonDTO>(person);
        }

        public async Task<List<PersonDTO>> GetPersonsAsync()
        {
            var persons = await _repository.GetAllAsync();
            return _mapper.Map<List<PersonDTO>>(persons);
        }

        public async Task UpdatePartialPropertiesPersonAsync(int ID, JsonPatchDocument<UpdatePersonDTO> patchPersonDTO)
        {
            var person = await _repository.GetByIdAsync(ID);
            if (person != null)
            {
                var patchedPerson = _mapper.Map<UpdatePersonDTO>(person);
                patchPersonDTO.ApplyTo(patchedPerson);

                _mapper.Map(patchedPerson, person);
                await _repository.UpdateAsync(person);
            }
        }

        public async Task UpdatePersonAsync(int ID, UpdatePersonDTO personDTO)
        {
            var person = await _repository.GetByIdAsync(ID);
            if (person != null)
            {
                _mapper.Map(personDTO, person);
                await _repository.UpdateAsync(person);
            }
        }
    }
}
