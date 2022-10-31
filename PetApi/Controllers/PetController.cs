using System.Collections.Generic;
using System.Linq;
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
        public PetDto FindPetByName([FromQuery]string name)
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
    }
}
