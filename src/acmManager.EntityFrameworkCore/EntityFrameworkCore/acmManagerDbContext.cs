using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using acmManager.Authorization.Roles;
using acmManager.Authorization.Users;
using acmManager.MultiTenancy;

namespace acmManager.EntityFrameworkCore
{
    public class acmManagerDbContext : AbpZeroDbContext<Tenant, Role, User, acmManagerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public acmManagerDbContext(DbContextOptions<acmManagerDbContext> options)
            : base(options)
        {
        }
    }
}
