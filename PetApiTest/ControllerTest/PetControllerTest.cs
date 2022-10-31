using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetControllerTest
    {
        [Fact]
        public async Task Should_add_new_pet_to_system_successfullyAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/releaseAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *   "name": "Kitty",
             *   "type": "cat",
             *   "color": "white",
             *   "price": 1000
             * }
             */

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/addNewPet", postbody);
            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async Task Should_get_all_pets_to_system_successfullyAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/releaseAllPets");

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postbody);
            // when
            var response = await httpClient.GetAsync("/api/getAllPets");
            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async Task Should_find_pet_by_name_successfullyAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/releaseAllPets");

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postbody);
            // when
            var response = await httpClient.GetAsync("/api/findPetByName?name=kitty");
            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var expectedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, expectedPet);
        }

        [Fact]
        public async Task Should_modify_price_sucessfullyAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/releaseAllPets");

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postbody);

            var expensivePet = new Pet("kitty", "cat", "white", 10000);
            var serializeExpensivePet = JsonConvert.SerializeObject(expensivePet);
            var putbody = new StringContent(serializeExpensivePet, Encoding.UTF8, "application/json");
            // when
            var response = await httpClient.PutAsync("/api/changePetPrice", putbody);
            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var expectedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(expensivePet, expectedPet);

            var getResponse = await httpClient.GetAsync("/api/findPetByName?name=kitty");
            getResponse.EnsureSuccessStatusCode();
            var getResponseBody = await getResponse.Content.ReadAsStringAsync();
            var updatedPet = JsonConvert.DeserializeObject<Pet>(getResponseBody);
            Assert.Equal(expensivePet, updatedPet);
        }
    }
}
