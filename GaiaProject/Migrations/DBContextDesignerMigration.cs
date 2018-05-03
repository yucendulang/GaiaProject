using GaiaProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaProject.Migrations
{
    public class DBContextDesignerMigration : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("server=localhost;database=GaiaProjectSQL; uid=sa; pwd=master;MultipleActiveResultSets=True");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
