using CRUD_Persons.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Persons.Interfaces
{
    public interface IPersonService
    {
        Task<List<PersonDTO>> GetPersonsAsync();
        Task<PersonDTO> GetPersonByIDAsync(int ID);
        Task<CreatePersonDTO> CreatePersonAsync(CreatePersonDTO createdPersonDTO);
        Task DeletePersonAsync(int ID);
        Task UpdatePersonAsync(int ID, UpdatePersonDTO personDTO);
        Task UpdatePartialPropertiesPersonAsync(int ID, JsonPatchDocument<UpdatePersonDTO> patchPersonDTO);
    }
}
