using BlazorStudentApp.Data.Models;
using BlazorStudentApp.Data.Services;
using BlazorStudentApp.Pages.Student;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorStudentApp.Tests;

public class Module4Tests
{
    [Fact]
    public void Submit_Event_ShouldFail_NewStudentComponent()
    {
        var student = new Student()
        {
            Id = 1,
            Name = "Ervis Trupja",
            Email = "ervis@trupja.com",
            Phone = "12345678",
            Address = "Address sample value"
        };
        var mockedService = new Mock<IStudentsService>();
        mockedService.Setup(s => s.AddStudentAsync(student)).ReturnsAsync(student);

        using var testContext = new TestContext();
        testContext.Services.AddScoped(provider => mockedService.Object);

        var renderedComponent = testContext.RenderComponent<NewStudent>();

        var submitButton = renderedComponent.Find("button[type=submit]");
        submitButton.Click();

        Assert.Contains(@"<div class=""validation-message"">The Name field is required.</div>",
            renderedComponent.Markup);
        Assert.Contains(@"<div class=""validation-message"">The Email field is required.</div>",
            renderedComponent.Markup);
        Assert.Contains(@"<div class=""validation-message"">The Phone field is required.</div>",
            renderedComponent.Markup);
        Assert.Contains(@"<div class=""validation-message"">The Address field is required.</div>",
            renderedComponent.Markup);

        Assert.False(renderedComponent.Instance.isSuccessMessageVisible);
    }

    [Fact]
    public void Submit_Event_ShouldSucceed_NewStudentComponent()
    {
        var expectedStudentFullName = "Ervis Trupja";
        var student = new Student()
        {
            Id = 1,
            Name = expectedStudentFullName,
            Email = "ervis@trupja.com",
            Phone = "12345678",
            Address = "Address sample value"
        };
        var mockedService = new Mock<IStudentsService>();
        mockedService.Setup(s => s.AddStudentAsync(student)).ReturnsAsync(student);

        using var testContext = new TestContext();
        testContext.Services.AddScoped(provider => mockedService.Object);

        var renderedComponent = testContext.RenderComponent<NewStudent>();

        renderedComponent.Find("#name").Change(expectedStudentFullName);
        renderedComponent.Find("#email").Change("ervis@trupja.com");
        renderedComponent.Find("#phone").Change("12345678");
        renderedComponent.Find("#address").Change("Ervis Trupja sample address");


        var submitButton = renderedComponent.Find("button[type=submit]");
        submitButton.Click();

        Assert.True(renderedComponent.Instance.isSuccessMessageVisible);
        Assert.Equal(expectedStudentFullName, renderedComponent.Instance.studentName);
    }

    [Fact]
    public void Render_NewStudentComponent()
    {
        var student = new Student();

        var mockedService = new Mock<IStudentsService>();
        mockedService.Setup(s => s.AddStudentAsync(student)).ReturnsAsync(student);

        using var testContext = new TestContext();
        testContext.Services.AddScoped(provider => mockedService.Object);

        // render component first time
        var renderedComponent = testContext.RenderComponent<NewStudent>();

        // render component second time
        renderedComponent.Render();
// verify render count of the component
        Assert.Equal(2, renderedComponent.RenderCount);
    }

    [Fact]
    public void AsyncAwait_Parameter_Render_IndexComponent()
    {
        using var context = new TestContext();
        var textService = new TaskCompletionSource<string>();
        var renderedComponent = context.RenderComponent<Pages.Index>(p =>
            p.Add(ts => ts.TextService, textService.Task));

        var expectedStringValue = "Welcome To Blazor Unit Testing";
        textService.SetResult(expectedStringValue);

        renderedComponent.WaitForState(() => renderedComponent.Find("h6").TextContent == expectedStringValue);

        Assert.Contains(@"<h6>" + expectedStringValue + "</h6>", renderedComponent.Markup);
    }

    [Fact]
    public void ExceptionThrown_NewStudentComponent()
    {
        var student = new Student();
        var mockedService = new Mock<IStudentsService>();
        mockedService.Setup(s => s.AddStudentAsync(student)).ReturnsAsync(student);

        using var testContext = new TestContext();
        testContext.Services.AddScoped(provider => mockedService.Object);

        var renderedComponent = testContext.RenderComponent<NewStudent>();
        //
        // var submitButton = renderedComponent.Find("button[type=submitdoesnotexist]");
        // submitButton.Click();

        Assert.Throws<Bunit.ElementNotFoundException>(() => renderedComponent.Find("button[type=submitdoesnotexist]"));
    } 
}