using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System.Collections.Generic;

namespace PetApi.Controllers
{
    
    [ApiController]
    [Route("api")]
    public class PetController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("findPetByName")]
        public Pet FindPetByName([FromQuery] string name)
        {
            return pets.Find(_ => _.Name == name);
        }

        [HttpDelete("deletePetByName")]
        public bool DeletePetByName([FromQuery] string name)
        {
            Pet deletePet = pets.Find(_ => _.Name == name);
            return pets.Remove(deletePet);
            
        }

        [HttpPut("modifyPetPrice")]
        public Pet ModifyPetByPrice(Pet pet)
        {
            var currentPet = pets.Find(_ => _.Name == pet.Name);
            currentPet.Price = pet.Price;
            return currentPet;

        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }
    }
}
