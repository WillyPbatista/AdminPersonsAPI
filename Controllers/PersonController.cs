using CRUD_Persons.DTOs;
using CRUD_Persons.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPersons()
        {
            try
            {
                var persons = await _personService.GetPersonsAsync();
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching persons.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("{ID:int}", Name = "GetPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDTO>> GetPersonByID(int ID)
        {
            if (ID == 0)
            {
                _logger.LogError("The person's ID is 0");
                return BadRequest("Invalid ID.");
            }

            try
            {
                var person = await _personService.GetPersonByIDAsync(ID);

                if (person == null)
                {
                    _logger.LogError("This person doesn't exist");
                    return NotFound();
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the person.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreatePersonDTO>> CreatePerson([FromBody] CreatePersonDTO createdPersonDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createdPersonDTO == null)
            {
                return BadRequest("Person data is null.");
            }

            try
            {
                var newPerson = await _personService.CreatePersonAsync(createdPersonDTO);

                if (newPerson == null)
                {
                    return BadRequest("This person already exists.");
                }

                return CreatedAtRoute("GetPerson", new { ID = newPerson.ID }, newPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the person.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpDelete("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePerson(int ID)
        {
            if (ID <= 0)
            {
                _logger.LogError("This ID is invalid because it is a negative number");
                return BadRequest("Invalid ID.");
            }

            try
            {
                await _personService.DeletePersonAsync(ID);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the person.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpPut("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePerson(int ID, [FromBody] UpdatePersonDTO upersonDTO)
        {
            if (ID != upersonDTO.ID || upersonDTO == null)
            {
                return BadRequest("Invalid person data.");
            }

            try
            {
                await _personService.UpdatePersonAsync(ID, upersonDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the person.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }

        [HttpPatch("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartialPropertiesPerson(int ID, JsonPatchDocument<UpdatePersonDTO> patchPersonDTO)
        {
            if (ID == 0 || patchPersonDTO == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                await _personService.UpdatePartialPropertiesPersonAsync(ID, patchPersonDTO);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the person.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error. Please try again later.");
            }
        }
    }
}
