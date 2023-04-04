using BlazorStudentApp.Data.Models;
using BlazorStudentApp.Data.Services;
using BlazorStudentApp.Pages.Student;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorStudentApp.Tests;

public class Module3Tests
{
    [Fact]
    public void Basic_Markup_IndexComponentRendersCorrectly()
    {
        using var context = new TestContext();

        var renderedComponent = context.RenderComponent<Pages.Index>();

        renderedComponent.MarkupMatches(
            "<h1>Welcome to our school</h1>\r\n<p>This is the official page to manage students of our School.</p>");
    }

    [Fact]
    public void DOM_Markup_IndexComponentRendersCorrectly()
    {
        using var context = new TestContext();

        var renderedComponent = context.RenderComponent<Pages.Index>();

        var h1Element = renderedComponent.Find("h1");
        h1Element.MarkupMatches("<h1>Welcome to our school</h1>");

        var pElement = renderedComponent.Find("p");
        pElement.MarkupMatches("<p>This is the official page to manage students of our School.</p>");
    }

    [Fact]
    public void Parameter_Passing_DetailsComponentRendersCorrectly_Test()
    {
        using var context = new TestContext();

        var expectedHeading = "Student details";
        var expectedStudent = new Student()
        {
            Id = 1,
            Name = "Ervis Trupja",
            Phone = "12345678",
            Address = "Address sample value"
        };
        var renderedComponent = context.RenderComponent<DetailsStudent>(
            p => p
                .Add(n => n.Heading, expectedHeading)
                .Add(n => n.SelectedStudent, expectedStudent));

        Assert.Contains( @"<h4 class=""modal-title"">" + expectedHeading + "</h4>", renderedComponent.Markup);
        Assert.Contains( @"<p class=""card-text"">" + expectedStudent.Address + "</p>",renderedComponent.Markup);
        Assert.Contains( @"<h5 class=""card-title"">" + expectedStudent.Name + "</h5>",renderedComponent.Markup);
        
        Assert.Equal(expectedHeading, renderedComponent.Instance.Heading);
        Assert.Equal(expectedStudent, renderedComponent.Instance.SelectedStudent);
    }

    [Fact]
    public void Service_Injection_AllStudentComponentRendersCorrectly()
    {
        var studentsList = new List<Student>()
        {
            new Student()
            {
                Id = 1,
                Name = "Ervis Trupja",
                Phone = "12345678",
                Address = "Address sample value"
            }
        };
        var mockedService = new Mock<IStudentsService>();
        mockedService.Setup(s => s.GetStudentsAsync()).ReturnsAsync(studentsList);

        using var context = new TestContext();

        context.Services.AddScoped(provider => mockedService.Object);

        var renderedComponent = context.RenderComponent<AllStudents>();

        Assert.Equal(studentsList.Count, renderedComponent.Instance.students.Count);
    }
}