using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace layoutlovers.EntityFrameworkCore
{
    public static class layoutloversDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<layoutloversDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<layoutloversDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}