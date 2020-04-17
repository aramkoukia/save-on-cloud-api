using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SaveOnCloud.Infrastructure.Data
{
    public class SaveOnCloudDbContextFactory : IDesignTimeDbContextFactory<SaveOnCloudDbContext>
    {
        public SaveOnCloudDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SaveOnCloudDbContext>();
            string localConnection = "Server=(localdb)\\mssqllocaldb;Database=SaveOnCloud;Trusted_Connection=True;Application Name=SaveOnCloud;";
            optionsBuilder.UseSqlServer(localConnection);
            return new SaveOnCloudDbContext(optionsBuilder.Options);
        }
    }
}
