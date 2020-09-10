using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace acmManager.EntityFrameworkCore
{
    public static class acmManagerDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<acmManagerDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<acmManagerDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
