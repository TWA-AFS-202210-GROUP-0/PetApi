using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        private static List<Pet> Pets { get; set; } = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            Pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPets()
        {
            return Pets;
        }

        [HttpGet("getPetByName")]
        public Pet FindPetByName([FromQuery] string name)
        {
            return Pets.Find(p => p.Name.Equals(name, System.StringComparison.Ordinal));
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            Pets.Clear();
        }
    }
}