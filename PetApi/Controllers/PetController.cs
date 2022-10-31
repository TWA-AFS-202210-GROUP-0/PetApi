using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using PetApi.Dto;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : ControllerBase
    {
        static private List<PetDto> pets = new List<PetDto>();

        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost("addNewPet")]
        public PetDto AddNewPet(PetDto pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<PetDto> GetAllPets()
        {
            return pets;
        }

        [HttpGet("findPetByName")]
        public PetDto FindPetByName([FromQuery] string name)
        {
            return pets.FirstOrDefault(e => e.Name == name);
        }

        [HttpDelete("deleteAll")]
        public void DeleteAll()
        {
            pets.Clear();
        }

        [HttpDelete("deleteByName")]
        public string DeleteByName([FromQuery] string name)
        {
            pets.RemoveAll(e => e.Name == name);
            return "Pet sold";
        }

        [HttpPut("changePrice")]
        public PetDto ChangePrice(PetDto pet)
        {
            var petToBeUpdated = pets.FirstOrDefault(e => e.Name == pet.Name);
            if (petToBeUpdated != null)
            {
                petToBeUpdated.Price = pet.Price;
            }
            else
            {
                pets.Add(pet);
            }

            return pet;
        }

        [HttpGet("getPetsByType")]
        public List<PetDto> GetPetsByType([FromQuery] string type)
        {
            return pets.Where(e => e.Type == type).ToList();
        }

        [HttpGet("getPetsByColor")]
        public List<PetDto> GetPetsByColor([FromQuery] string color)
        {
            return pets.Where(e => e.Color == color).ToList();
        }

        [HttpGet("getPetsByPrice")]
        public List<PetDto> GetPetsByColor([FromQuery] double low, double high)
        {
            return pets.Where(e => e.Price > low && e.Price < high).ToList();
        }
    }
}
