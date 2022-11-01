using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetApiTest.ControllerTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class PetController : ControllerBase
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

        [HttpGet("getPetByType")]
        public List<Pet> GetPetByType([FromQuery] string type)
        {
            return pets.Where(pet => pet.Type == type).ToList();
        }

        [HttpGet("getPetsByRange")]
        public List<Pet> GetPetsByRange([FromQuery] double minPrice, [FromQuery] double maxPrice)
        {
            return pets.Where(pet => (pet.Price >= minPrice && pet.Price <= maxPrice)).ToList();
        }

        [HttpGet("getPetsByColor")]
        public List<Pet> GetPetsByColor([FromQuery] string color)
        {
            return pets.Where(pet => pet.Color == color).ToList();
        }

        [HttpGet("findPetByName")]
        public Pet FindPetByName([FromQuery] string name)
        {
            return pets.Find(pet => pet.Name == name);
        }

        [HttpPut("changePetPrice")]
        public Pet ChangePetPrice(Pet pet)
        {
            var oldPricePet = pets.Find(p => p.Name == pet.Name);
            pets.Remove(oldPricePet);
            pets.Add(pet);
            return pet;
        }

        [HttpDelete("buyPet")]
        public Pet BuyPet([FromQuery]string name)
        {
            var boughtPet = pets.Find(p => p.Name == name);
            try
            {
                pets.Remove(boughtPet);
            }
            catch (Exception e)
            {
                return null;
            }
            
            return boughtPet;
        }

        [HttpDelete("releaseAllPets")]
        public void ReleaseAllPets()
        {
            pets.Clear();
        }
    }
}
