using System.Data.Entity.Migrations;
using MyTempProject.Migrations.Seed;

namespace MyTempProject.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MyTempProject.EntityFramework.AbpZeroTemplateDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AbpZeroTemplate";
        }

        protected override void Seed(MyTempProject.EntityFramework.AbpZeroTemplateDbContext context)
        {
            new InitialDbBuilder(context).Create();
            //new DefaultOrganizationUnitCreator(context).Create();
        }
    }
}
