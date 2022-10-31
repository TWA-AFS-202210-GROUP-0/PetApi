using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
    [Fact]
    public async void Should_add_new_pet_to_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        //when
        var response = await httpClient.PostAsync("/api/addNewPet", postBody);
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_get_all_pets_from_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/getAllPets");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(pet, allPets[0]);
    }

    [Fact]
    public async void Should_get_pet_by_name_from_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet1 = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var pet2 = new Pet(name: "WangCai", type: "dog", color: "white", price: 5000);
        var serializeObject = JsonConvert.SerializeObject(pet1);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        var serializeObject2 = JsonConvert.SerializeObject(pet2);
        var postBody2 = new StringContent(serializeObject2, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody2);
        //when
        var response = await httpClient.GetAsync("/api/findPetByName?name=Kitty");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var findPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet1, findPet);
    }

    [Fact]
    public async void Should_delete_pet_by_name_from_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.DeleteAsync("/api/deletePetByName?name=Kitty");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var isSuccess = JsonConvert.DeserializeObject<bool>(responseBody);
        Assert.True(isSuccess);
        var responseGet = await httpClient.GetAsync("/api/findPetByName?name=Kitty");
        responseGet.EnsureSuccessStatusCode();
        var responseGetBody = await responseGet.Content.ReadAsStringAsync();
        var findPet = JsonConvert.DeserializeObject<Pet>(responseGetBody);
        Assert.Equal(null, findPet);
    }

    [Fact]
    public async void Should_modify_pet_given_a_pet_in_system()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        Pet modifiedPricePet = new Pet(name: "Kitty", type: "cat", color: "white", price: 5000);
        var serializeObjectModified = JsonConvert.SerializeObject(modifiedPricePet);
        var postBodyModified = new StringContent(serializeObjectModified, Encoding.UTF8, "application/json");
        //when
        await httpClient.PutAsync("/api/modifyPetPrice", postBodyModified);
        //then
        var response = await httpClient.GetAsync("/api/findPetByName?name=Kitty");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var modifiedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(5000, modifiedPet.Price);
    }

    [Fact]
    public async void Should_find_pet_by_price_range_from_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet1 = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject1 = JsonConvert.SerializeObject(pet1);
        var postBody1 = new StringContent(serializeObject1, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody1);
        var pet2 = new Pet(name: "WangCai", type: "dog", color: "white", price: 5000);
        var serializeObject2 = JsonConvert.SerializeObject(pet2);
        var postBody2 = new StringContent(serializeObject2, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody2);
        var pet3 = new Pet(name: "Puppy", type: "dog", color: "black", price: 3000);
        var serializeObject3 = JsonConvert.SerializeObject(pet3);
        var postBody3 = new StringContent(serializeObject3, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody3);
        //when
        var response = await httpClient.GetAsync("/api/findPetsByPriceRange?minPrice=1000&maxPrice=4000");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var petsByPrinceRange = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(new List<Pet>() { pet1, pet3 }, petsByPrinceRange);
    }
}
