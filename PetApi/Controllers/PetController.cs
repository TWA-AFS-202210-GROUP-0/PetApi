using Microsoft.AspNetCore.Mvc;
using PetApi;
using System.Collections.Generic;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : Controller
    {
        private static List<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPet()
        {
            return pets;
        }

        [HttpGet("findPetByName")]
        public Pet FindGetByName([FromQuery] string name)
        {
            return pets.Find(pet => pet.Name == name);
        }

        [HttpGet("findPetByType")]
        public List<Pet> FindGetByType([FromQuery] string type)
        {
            return pets.FindAll(pet => pet.Type == type);
        }

        [HttpDelete("deletePetByName")]
        public List<Pet> DeleteByName([FromQuery] string name)
        {
            var pet = pets.Find(pet => pet.Name == name);
            pets.Remove(pet);
            return pets;
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpPut("modifyPriceoOfPet")]
        public Pet ModifyPriceByName([FromQuery] string name, [FromBody] Pet pet)
        {
            var oldPet = pets.Find(pet => pet.Name == name);
            oldPet.Price = pet.Price;
            return oldPet;
        }
    }
}
