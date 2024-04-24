using CRUD_Persons.Data;
using CRUD_Persons.DTOs;
using CRUD_Persons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Persons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PersonDTO>> GetPersons() 
            {
                return Ok(VillaStore.PersonList);
            }

        [HttpGet("ID:int", Name = "GetPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonDTO> GetPersonByID(int ID)
            {
            if (ID == 0)
                return BadRequest();

            var person = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);

            if (person == null)
                return NotFound();

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
                return BadRequest(personDTO);
            
            if(personDTO.ID > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            personDTO.ID = VillaStore.PersonList.OrderByDescending(x => x.ID).FirstOrDefault().ID + 1;

            VillaStore.PersonList.Add(personDTO);

            return CreatedAtRoute("GetPerson", new { ID = personDTO.ID }, personDTO);
        }

        [HttpDelete("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePerson(int ID)
        {
            if (ID <= 0)
                return BadRequest();

            var deletedPerson = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);

            if (deletedPerson == null) 
            {
                return NotFound(); 
            }

            VillaStore.PersonList.Remove(deletedPerson);
            return NoContent();

        }

        [HttpPut("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePerson(int ID, [FromBody] PersonDTO personDTO)
        {
            if (ID != personDTO.ID || personDTO == null)
                return BadRequest();

            var updatedPerson = VillaStore.PersonList.FirstOrDefault(x => x.ID == ID);

            if (updatedPerson == null)
                return NotFound();

            updatedPerson.Name = personDTO.Name;
            updatedPerson.LastName = personDTO.LastName;
            updatedPerson.Age = personDTO.Age;

            return NoContent();
        }

    }
}
