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

        [Fact]
        public async Task Should_buy_pet_sucessfullyAsync()
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
            var response = await httpClient.DeleteAsync("/api/buyPet?name=kitty");
            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var expectedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, expectedPet);
        }

        [Fact]
        public async Task Should_get_pets_by_type_sucessfullyAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/releaseAllPets");

            var cat1 = new Pet("kitty1", "cat", "white", 1000);
            var serializeObject = JsonConvert.SerializeObject(cat1);
            var cat1Postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", cat1Postbody);

            var cat2 = new Pet("kitty2", "cat", "white", 1000);
            serializeObject = JsonConvert.SerializeObject(cat2);
            var cat2Postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", cat2Postbody);

            var dog = new Pet("doggy", "dog", "white", 1000);
            serializeObject = JsonConvert.SerializeObject(dog);
            var dogPostbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", dogPostbody);

            // when
            var response = await httpClient.GetAsync("/api/getPetByType?type=cat");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var expectedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, expectedPets.Count);
            Assert.Equal("cat", expectedPets[0].Type);
            Assert.Equal("cat", expectedPets[1].Type);
        }

        [Fact]
        public async Task Should_get_pets_by_price_range_sucessfullyAsync()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/releaseAllPets");

            var cat1 = new Pet("kitty1", "cat", "white", 200);
            var serializeObject = JsonConvert.SerializeObject(cat1);
            var cat1Postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", cat1Postbody);

            var cat2 = new Pet("kitty2", "cat", "white", 300);
            serializeObject = JsonConvert.SerializeObject(cat2);
            var cat2Postbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", cat2Postbody);

            var dog = new Pet("doggy", "dog", "white", 1000);
            serializeObject = JsonConvert.SerializeObject(dog);
            var dogPostbody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", dogPostbody);

            // when
            var response = await httpClient.GetAsync("/api/getPetsByRange?minPrice=100&maxPrice=400");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var expectedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, expectedPets.Count);
            Assert.InRange(expectedPets[0].Price, 100, 400);
            Assert.InRange(expectedPets[1].Price, 100, 400);
        }
    }
}
