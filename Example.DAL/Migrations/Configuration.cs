namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Example.DAL.ExampleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Example.DAL.ExampleDbContext context)
        {
	        if (context.Users.Any())
				return;
            var seed = new DatabaseSeed(context);
            seed.SeedUsersAndRoles();
        }
    }
}
