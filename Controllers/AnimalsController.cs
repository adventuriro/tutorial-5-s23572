using tutorial_5_s23572.Models;
using Microsoft.AspNetCore.Mvc;

namespace tutorial_5_s23572.Controllers
{
    [ApiController]
    [Route("/api/animals")]
    public class AnimalsController : ControllerBase
    {
        private static readonly string[] AllowedSortKeys =
        {
            "Name",
            "Description",
            "Category",
            "Area",
        };
        
        private readonly Database _db;

        public AnimalsController(Database db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? orderBy)
        {
            if (!AllowedSortKeys.Contains(orderBy))
            {
                return BadRequest($"orderBy does not point at an allowed key");
            }
            
            return Ok(await _db.GetAnimals(orderBy));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var animal = await _db.GetAnimalById(id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Animal animal)
        {
            if (animal.IdAnimal != default)
            {
                return BadRequest("Cannot specify record ID");
            }
            
            await _db.CreateAnimal(animal);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Animal animal)
        {
            if (animal.IdAnimal != default)
            {
                return BadRequest("Cannot modify record ID");
            }

            animal.IdAnimal = id;
            await _db.UpdateAnimal(animal);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _db.DeleteAnimal(id);
            return Ok();
        }
    }
}
