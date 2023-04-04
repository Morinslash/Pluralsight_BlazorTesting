using BlazorStudentApp.Data.Models;

namespace BlazorStudentApp.Data
{
    public static class AppDbInitializer
    {

        public static void Seed(IApplicationBuilder appBuilder)
        {

            var scopeeee = appBuilder.ApplicationServices.CreateScope();
            ApplicationDbContext context = scopeeee.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Students.Any())
            {
                context.AddRange
                (
                    new Student
                    {
                        Name = "John Doe",
                        Email = "john@doe.com",
                        Phone = "555-555-5555",
                        Address = "123 Main St"
                    },
                    new Student()
                    {
                        Name = "Jane Doe",
                        Email = "jane@doe.com",
                        Phone = "555-555-5555",
                        Address = "123 Main St"
                    },
                    new Student()
                    {
                        Name = "John Smith",
                        Email = "john@smith.com",
                        Phone = "555-555-5555",
                        Address = "123 Main St"
                    },
                    new Student()
                    {
                        Name = "Jane Smith",
                        Email = "jane@smith.com",
                        Phone = "555-555-5555",
                        Address = "123 Main St"
                    });
            }

            context.SaveChanges();
        }
    }
}
