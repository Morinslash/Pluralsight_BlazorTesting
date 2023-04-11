using BlazorStudentApp.Data.Models;
using BlazorStudentApp.Tests.Helpers;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace BlazorStudentApp.Tests;

public class Module5Tests
{
    [Fact]
    public void UserNotAuthenticated_IndexComponent()
    {
        using var testContext = new TestContext();
        testContext.AddTestAuthorization();

        var renderedComponent = testContext.RenderComponent<Pages.Index>();

        Assert.Contains(@"<h1>Please log in!</h1>", renderedComponent.Markup);
    }
    
    [Fact]
    public void UserAuthenticated_IndexComponent()
    {
        using var testContext = new TestContext();
        var authContext = testContext.AddTestAuthorization();
        authContext.SetAuthorized("ervis@trupja.com", AuthorizationState.Authorized);

        var renderedComponent = testContext.RenderComponent<Pages.Index>();

        Assert.Contains(@"<h1>Welcome to our school</h1>", renderedComponent.Markup);
    }

    [Fact]
    public async Task HttpClient_Request_SuccessResponse_Mock_Test()
    {
        var studentsList = new List<Student>()
        {
            new Student()
            {
                Id = 1,
                Name = "Ervis Trupja",
                Email = "ervis@trupja.com",
                Phone = "12345678",
                Address = "Address sample value"
            }
        };
        var testContext = new TestContext();
        var mockHttpMessageHandler = testContext.Services.AddMockHttpClient();
        mockHttpMessageHandler.When("/getAllStudents").RespondJson(studentsList);

        var httpClient = testContext.Services.BuildServiceProvider().GetService<HttpClient>();
        var httpClientResponse = await httpClient.GetAsync("/getAllStudents");
        httpClientResponse.EnsureSuccessStatusCode();

        string jsonContent = await httpClientResponse.Content.ReadAsStringAsync();
        List<Student> httpResponseStudents = JsonConvert.DeserializeObject<List<Student>>(jsonContent);
        
        Assert.Equal(studentsList.First().Name, httpResponseStudents.First().Name);
    }

    [Fact]
    public void NavigationManager_Navigate()
    {
        string expectedNavigateToUrl = "welcome";
        
        using var testContext = new TestContext();
        var authContext = testContext.AddTestAuthorization();
        authContext.SetAuthorized("ervis@trupja.com", AuthorizationState.Authorized);

        var navigationManager = testContext.Services.GetRequiredService<FakeNavigationManager>();
        var renderComponent = testContext.RenderComponent<Pages.Index>();
        
        navigationManager.NavigateTo(expectedNavigateToUrl);
        
        Assert.Equal($"http://localhost/{expectedNavigateToUrl}", navigationManager.Uri);
    }
}