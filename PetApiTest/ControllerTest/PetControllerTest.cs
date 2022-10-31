using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PetApi.Dto;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
    public (WebApplicationFactory<Program>, HttpClient) SetUpEnviroment()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        httpClient.DeleteAsync("api/deleteAll");
        return (application, httpClient);
    }

    public StringContent BuildRequestBody(object obj)
    {
        var serializedObject = JsonConvert.SerializeObject(obj);
        return new StringContent(serializedObject, Encoding.UTF8, "application/json");
    }

    [Fact]
    public async void Should_add_new_pet_()
    {
        //Given
        var (application, httpClient) = SetUpEnviroment();

        var pet = new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 1000,
        };
        //When
        var requestBody = BuildRequestBody(pet);
        var respone = await httpClient.PostAsync("/api/addNewPet", requestBody);

        //Then
        respone.EnsureSuccessStatusCode();
        var responseBody = await respone.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<PetDto>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_get_all_pets()
    {
        //Given
        var (application, httpClient) = SetUpEnviroment();

        var pet = new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 1000,
        };
        var postBody = BuildRequestBody(pet);
        var postResponse = await httpClient.PostAsync("/api/addNewPet", postBody);
        //When
        var response = await httpClient.GetAsync("/api/getAllPets");

        //Then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var allPets = JsonConvert.DeserializeObject<List<PetDto>>(responseBody);
        Assert.Equal(pet, allPets[0]);
    }

    [Fact]
    public async void Should_get_pet_by_name()
    {
        //Given
        var (application, httpClient) = SetUpEnviroment();

        var pet = new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 1000,
        };
        var postBody = BuildRequestBody(pet);
        var postResponse = await httpClient.PostAsync("/api/addNewPet", postBody);
        //When
        var getResponse = await httpClient.GetAsync("/api/findPetByName?name=Mengyu");

        //Then
        getResponse.EnsureSuccessStatusCode();
        var responseBody = await getResponse.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<PetDto>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_delete_pet_when_purchased()
    {
        //Given
        var (application, httpClient) = SetUpEnviroment();

        var pet = new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 1000,
        };
        var postBody = BuildRequestBody(pet);
        var postResponse = await httpClient.PostAsync("/api/addNewPet", postBody);
        //When
        var deleteResponse = await httpClient.DeleteAsync("/api/deleteByName?name=Mengyu");

        //Then
        deleteResponse.EnsureSuccessStatusCode();
        var responseBody = await deleteResponse.Content.ReadAsStringAsync();
        Assert.Equal("Pet sold", responseBody);
    }

    [Fact]
    public async void Should_change_price_when_put()
    {
        //Given
        var (application, httpClient) = SetUpEnviroment();

        var pet = new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 1000,
        };
        var postBody = BuildRequestBody(pet);
        var postResponse = await httpClient.PostAsync("/api/addNewPet", postBody);
        //When
        var putBody = BuildRequestBody(new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 2000,
        });
        var putRespone = await httpClient.PutAsync("api/changePrice", putBody);
        //Then
        var getResponse = await httpClient.GetAsync("/api/findPetByName?name=Mengyu");
        getResponse.EnsureSuccessStatusCode();
        var responseBody = await getResponse.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<PetDto>(responseBody);
        Assert.Equal(2000, savedPet.Price);
    }

    [Fact]
    public async void Should_get_pet_by_type_is_dog()
    {
        //Given
        var (application, httpClient) = SetUpEnviroment();

        var pet = new PetDto()
        {
            Name = "Mengyu",
            Type = "Dog",
            Color = "Blue",
            Price = 1000,
        };
        var pet2 = new PetDto()
        {
            Name = "Yanxi",
            Type = "Dog",
            Color = "Black",
            Price = 1000,
        };
        var pet3 = new PetDto()
        {
            Name = "Meng",
            Type = "Cat",
            Color = "Black",
            Price = 1000,
        };

        var postBody = BuildRequestBody(pet);
        var postBody2 = BuildRequestBody(pet2);
        var postBody3 = BuildRequestBody(pet3);
        var postResponse = await httpClient.PostAsync("/api/addNewPet", postBody);
        _ = await httpClient.PostAsync("/api/addNewPet", postBody2);
        _ = await httpClient.PostAsync("/api/addNewPet", postBody3);
        //When
        var getResponse = await httpClient.GetAsync("/api/getPetsByType?type=Dog");

        //Then
        getResponse.EnsureSuccessStatusCode();
        var responseBody = await getResponse.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<List<PetDto>>(responseBody);
        Assert.Equal(2, savedPet.Count);
        Assert.Equal("Dog", savedPet[0].Type);
        Assert.Equal("Dog", savedPet[1].Type);
    }

}