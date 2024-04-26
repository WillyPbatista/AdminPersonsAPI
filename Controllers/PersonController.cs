using CRUD_Persons.Data;
using CRUD_Persons.DTOs;
using CRUD_Persons.Interfaces;
using CRUD_Persons.Models;
using CRUD_Persons.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Persons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;
        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PersonDTO>> GetPersons() 
            {
                return Ok(_personService.GetPersons());
            }

        [HttpGet("ID:int", Name = "GetPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonDTO> GetPersonByID(int ID)
            {
            if (ID == 0)
            {
                _logger.LogError("The person's ID is 0");
                return BadRequest();
            }


            var person = _personService.GetPersonByID(ID);

            if (person == null)
            {
                _logger.LogError("This person does'nt exist");
                return NotFound();
            }
                

                return Ok(person);
            }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> CreatePerson([FromBody] PersonDTO personDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            if(VillaStore.PersonList.FirstOrDefault(x => x.Name.ToLower() == personDTO.Name.ToLower()) != null 
                && 
               VillaStore.PersonList.FirstOrDefault(x => x.LastName.ToLower() == personDTO.LastName.ToLower()) != null)
            {
                ModelState.AddModelError("existPerson", "This Person already exist");
                return BadRequest(ModelState);
            }

            if (personDTO == null)
            {
                _logger.LogError("This person does'nt exist");
                return BadRequest(personDTO);
            }
                
            
            if(personDTO.ID > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var newPerson = _personService.CreatePerson(personDTO);

            return CreatedAtRoute("GetPerson", new { ID = newPerson.ID }, newPerson);
        }

        [HttpDelete("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeletePerson(int ID)
        {
            if (ID <= 0)
            {
                _logger.LogError("This ID is ivalid because is a negative number");
                return BadRequest();
            }
                

             _personService.DeletePerson(ID);

            return NoContent();

        }

        [HttpPut("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePerson(int ID, [FromBody] PersonDTO personDTO)
        {
            if (ID != personDTO.ID || personDTO == null)
                return BadRequest();

             _personService.UpdatePerson(ID, personDTO);

            return NoContent();
        }

        [HttpPatch("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialPropertiesPerson(int ID, JsonPatchDocument<PersonDTO> patchPersonDTO)
        {
            if (ID == 0|| patchPersonDTO == null)
                return BadRequest();

            _personService.UpdatePartialPropertiesPerson(ID,patchPersonDTO);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }
    }
}
