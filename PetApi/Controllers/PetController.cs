using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            return Pets.Find(p => p.Name.Equals(name, StringComparison.Ordinal));
        }

        [HttpGet("getPetByType")]
        public List<Pet> FindPetByType([FromQuery] string type)
        {
            return Pets.FindAll(p => p.Type.Equals(type));
        }

        [HttpGet("getPetByColor")]
        public List<Pet> FindPetByColor([FromQuery] string color)
        {
            return Pets.FindAll(p => p.Color.Equals(color));
        }

        [HttpGet("getPetByPrice")]
        public List<Pet> FindPetByPrice([FromQuery] int min, [FromQuery] int max)
        {
            return Pets.FindAll(p => p.Price >= min && p.Price <= max);
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            Pets.Clear();
        }

        [HttpDelete("deletePetByName")]
        public void DeletePetByName([FromQuery] string name)
        {
            Pets.RemoveAll(p => p.Name.Equals(name));
        }

        [HttpPatch("upatePetPrice")]
        public List<Pet> UpatePetPrice([FromBody] Pet pet)
        {
            var res = Pets.FirstOrDefault(n => n.Name == pet.Name);
            res.Price = pet.Price;
            return Pets;
        }
    }
}