﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Models;
using Xunit;

namespace PetApiTest
{
    public class PetControllerTest
    {
        [Fact]
        public async Task Should_add_new_pet_to_system_successful()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            // when
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/addNewPet", postBody);

            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savePet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savePet);
        }

        [Fact]
        public async Task Should_get_all_pets_from_system()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            // when
            var response = await httpClient.GetAsync("/api/getAllPets");

            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async Task Should_get_pet_by_name_from_system()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            // when
            var response = await httpClient.GetAsync("/api/getPetByName?name=Kitty");

            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var getPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, getPet);
        }

        [Fact]
        public async Task Should_delete_pet_if_pet_is_bought()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            // when
            await httpClient.DeleteAsync("/api/deletePetByName?name=Kitty");
            var response = await httpClient.DeleteAsync("/api/deletePetByName?name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var deleteResult = JsonConvert.DeserializeObject<bool>(responseBody);
            Assert.Equal(false, deleteResult);
        }

        [Fact]
        public async Task Should_modify_pet_price_given_pet_name_and_new_price()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            // when
            var putPet = new Pet(name: "Kitty", type: "cat", color: "white", price: 10000);
            var serializeObjectPutPet = JsonConvert.SerializeObject(putPet);
            var putBody = new StringContent(serializeObjectPutPet, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("/api/ModifyPet", putBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var editedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(putPet, editedPet);
        }

        [Fact]
        public async Task Should_get_pets_given_type()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            var pet1 = new Pet(name: "Mm", type: "cat", color: "white", price: 1000);
            var serializeObject1 = JsonConvert.SerializeObject(pet1);
            var postBody1 = new StringContent(serializeObject1, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody1);

            // when
            var response = await httpClient.GetAsync("/api/getPetsByType?type=cat");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var editedPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, editedPet[0]);
            Assert.Equal(pet1, editedPet[1]);
        }

        [Fact]
        public async Task Should_get_pets_sort_by_price()
        {
            // given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *      "name": "Kitty",
             *      "type": "",
             *      "color": "white",
             *      "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            var pet1 = new Pet(name: "Mm", type: "cat", color: "white", price: 2000);
            var serializeObject1 = JsonConvert.SerializeObject(pet1);
            var postBody1 = new StringContent(serializeObject1, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody1);

            // when
            var response = await httpClient.GetAsync("/api/getPetsSortByPrice");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var editedPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, editedPet[0]);
            Assert.Equal(pet1, editedPet[1]);
        }
    }
}
