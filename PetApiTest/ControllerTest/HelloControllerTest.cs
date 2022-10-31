using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http;
using System.Text;
using Xunit;
using PetApi;
using System.Collections.Generic;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
    [Fact]
    public async void Should_add_new_pet_to_the_system_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        /*
         * body:
         * {
         *  "name": "Kitty",
         *  "type" : "cat",
         *  "color" : "white",
         *  "price" : 1000
         * }
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("api/addNewPet", postBody);
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_get_all_pets_to_the_system_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        var pet = new Pet("Kitty", "cat", "white", 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/getAllPets");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(pet, allPets[0]);
    }

    [Fact]
    public async void Should_find_pet_by_name_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        var pet = new Pet("Kitty", "cat", "white", 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/findPetByName?name=Kitty");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_delete_the_first_of_all_pets_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var petOne = new Pet("Kitty", "cat", "white", 1000);
        var petTwo = new Pet("Doudou", "dog", "yellow", 2000);
        var serializeObjectOne = JsonConvert.SerializeObject(petOne);
        var serializeObjectTwo = JsonConvert.SerializeObject(petTwo);
        var postBodyone = new StringContent(serializeObjectOne, Encoding.UTF8, "application/json");
        var postBodytwo = new StringContent(serializeObjectTwo, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", postBodyone);
        await httpClient.PostAsync("api/addNewPet", postBodytwo);
        //when
        var response = await httpClient.DeleteAsync("/api/deletePetByName?name=Kitty");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var remainedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(petTwo, remainedPets[0]);
    }

    [Fact]
    public async void Should_modify_the_price_of_first_of_all_pets_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var petOne = new Pet("Kitty", "cat", "white", 1000);
        var petOneMd = new Pet("Kitty", "cat", "white", 2000);
        var petTwo = new Pet("Doudou", "dog", "yellow", 2000);
        var serializeObjectOne = JsonConvert.SerializeObject(petOne);
        var serializeObjectOneMd = JsonConvert.SerializeObject(petOneMd);
        var serializeObjectTwo = JsonConvert.SerializeObject(petTwo);
        var postBodyone = new StringContent(serializeObjectOne, Encoding.UTF8, "application/json");
        var postBodyoneMd = new StringContent(serializeObjectOneMd, Encoding.UTF8, "application/json");
        var postBodytwo = new StringContent(serializeObjectTwo, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", postBodyone);
        await httpClient.PostAsync("api/addNewPet", postBodytwo);
        //when
        var response = await httpClient.PutAsync("/api/modifyPriceoOfPet?name=Kitty", postBodyoneMd);
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var modifiedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(petOneMd, modifiedPet);
    }
   //
   //[Fact]
   //public async void Should_get_the_pet_by_type_successfully()
   //{
   //    //given
   //    var application = new WebApplicationFactory<Program>();
   //    var httpClient = application.CreateClient();
   //    var petOne = new Pet("Kitty", "cat", "white", 1000);
   //    var petTwo = new Pet("Miao", "cat", "yellow", 2000);
   //    var serializeObjectOne = JsonConvert.SerializeObject(petOne);
   //    var serializeObjectTwo = JsonConvert.SerializeObject(petTwo);
   //    var postBodyone = new StringContent(serializeObjectOne, Encoding.UTF8, "application/json");
   //    var postBodytwo = new StringContent(serializeObjectTwo, Encoding.UTF8, "application/json");
   //    await httpClient.PostAsync("api/addNewPet", postBodyone);
   //    await httpClient.PostAsync("api/addNewPet", postBodytwo);
   //    //when
   //    var response = await httpClient.GetAsync("/api/findPetByName?type=cat");
   //    //then
   //    response.EnsureSuccessStatusCode();
   //    var responseBody = await response.Content.ReadAsStringAsync();
   //    var savedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
   //    Assert.Equal(2, savedPets.Count);
   //}
}