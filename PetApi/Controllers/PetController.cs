using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.PortableExecutable;
using Force.DeepCloner;
using Microsoft.AspNetCore.Mvc;
using PetApi.Dto;
using PetApi.Model;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : ControllerBase
    {
        static private List<PetDto> pets = new List<PetDto>();

        [HttpGet("queryPets/")]
        public List<PetDto> QueryPets([FromQuery]PetQuery petQuery)
        {
            var selectedPets = pets.Select(e => e).ToList();
            if (petQuery.Type != null)
            {
                selectedPets.Where(e => e.Type == petQuery.Type).ToList();
            }

            if (petQuery.Color != null)
            {
                selectedPets.Where(e => e.Color == petQuery.Color).ToList();
            }

            if (petQuery.Low != null)
            {
                selectedPets.Where(e => e.Price > petQuery.Low).ToList();
            }

            if (petQuery.High != null)
            {
                selectedPets.Where(e => e.Price < petQuery.High).ToList();
            }

            return selectedPets;
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
    }
}
