namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;
    using RandomNameGenerator;

    internal sealed class Configuration : DbMigrationsConfiguration<MedAgenda.API.Infrastructure.MedAgendaDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MedAgenda.API.Infrastructure.MedAgendaDataContext context)
        {
            for(int i = 0; i < 100; i++)
            {
                context.Patients.Add(new Models.Patient
                {
                    FirstName = NameGenerator.GenerateFirstName(Gender.Male),
                    LastName = NameGenerator.GenerateLastName(),
                    DateOfBirth = DateTime.Parse("01/01/2000")
                });
                context.SaveChanges();
            }

            for(int i = 0; i < 10; i++)
            {
                context.Doctors.Add(new Models.Doctor
                {
                    FirstName = NameGenerator.GenerateFirstName(Gender.Male),
                    LastName = NameGenerator.GenerateLastName()
                });
                context.SaveChanges();
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
