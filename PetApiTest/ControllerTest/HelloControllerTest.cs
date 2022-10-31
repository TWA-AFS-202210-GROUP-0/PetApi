using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using PetApi;
using System.Collections.Generic;
using System.Net.Http.Json;
using Xunit;

namespace PetApiTest.ControllerTest;

public class HelloControllerTest
{
    [Fact]
    public async void Should_add_new_pet()
    {
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 100);

        var response = await client.PostAsJsonAsync("api/addNewPet", pet);
        string res = response.Content.ReadAsStringAsync().Result;
        var resPet = JsonConvert.DeserializeObject<Pet>(res);
        response.EnsureSuccessStatusCode();
        Assert.Equal(resPet, pet);
    }

    [Fact]
    public async void Should_get_all_pets()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 100);

        await client.PostAsJsonAsync("api/addNewPet", pet);

        // when
        var response = await client.GetAsync("api/getAllPets");
        string responseBody = response.Content.ReadAsStringAsync().Result;
        var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        response.EnsureSuccessStatusCode();
        Assert.Equal(allPets[0], pet);
    }

    [Fact]
    public async void Should_get_pet_by_name()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var client = application.CreateClient();
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 100);

        await client.PostAsJsonAsync("api/addNewPet", pet);
        await client.PostAsJsonAsync("api/addNewPet", new Pet(name: "ab", type: "cat", color: "white", price: 100));

        // when
        var response = await client.GetAsync("api/getPetByName?name=Kitty");
        string responseBody = response.Content.ReadAsStringAsync().Result;
        var resPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        response.EnsureSuccessStatusCode();
        Assert.Equal(resPet, pet);
    }
}