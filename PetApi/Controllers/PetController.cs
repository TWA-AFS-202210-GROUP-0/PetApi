using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetApi.Models;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        static private List<Pet> pets = new List<Pet>();

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

        [HttpGet("getPetByName")]
        public Pet GetPetByName([FromQuery] string name)
        {
            return pets.Find(_ => _.Name == name);
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAll()
        {
            pets.Clear();
        }

        [HttpDelete("deletePetByName")]
        public bool DeletePetByName([FromQuery] string name)
        {
            Pet pet = pets.Find(_ => _.Name == name);
            return pets.Remove(pet);
        }

        [HttpPut("ModifyPet")]
        public Pet ModifyPetByName([FromBody] Pet pet)
        {
            Pet removePet = pets.Find(_ => _.Name == pet.Name);
            pets.Remove(removePet);
            pets.Add(pet);
            return pet;
        }
    }
}
