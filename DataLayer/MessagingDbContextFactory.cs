using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace DataLayer
{
    ///<summary> migration context <summary>
    public class MessagingDbContextFactory : IDesignTimeDbContextFactory<MessagingContext>
    {
        public MessagingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessagingContext>();
            optionsBuilder.UseMySql(
                        // Replace with your connection string.
                        "Server=localhost;port=33066;Database=ef;user=user;password=password",
                        // Replace with your server version and type.
                        // For common usages, see pull request #1233.
                        new MySqlServerVersion(new Version(8, 0, 21)), // use MariaDbServerVersion for MariaDB
                        mySqlOptions => {
                            mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend);
                            mySqlOptions.EnableRetryOnFailure();
                        }
                        
                    )
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            return new MessagingContext(optionsBuilder.Options);
        }
    }
}
