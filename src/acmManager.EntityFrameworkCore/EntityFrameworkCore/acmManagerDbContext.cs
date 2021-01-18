using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using acmManager.Article;
using acmManager.Authorization.Roles;
using acmManager.Authorization.Users;
using acmManager.Contest;
using acmManager.MultiTenancy;
using acmManager.Problem;

namespace acmManager.EntityFrameworkCore
{
    public class acmManagerDbContext : AbpZeroDbContext<Tenant, Role, User, acmManagerDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        // User
        public DbSet<UserInfo> UserInfos { get; set; }
        
        // Article
        public DbSet<Article.Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        // File
        public DbSet<File.File> Files { get; set; }
        
        // Problem
        public DbSet<Problem.Problem> Problems { get; set; }
        public DbSet<ProblemSolution> ProblemSolutions { get; set; }
        public DbSet<ProblemType> ProblemTypes { get; set; }
        public DbSet<RecommendVote> RecommendVotes { get; set; }
        
        // Contest
        public DbSet<Contest.Contest> Contests { get; set; }
        public DbSet<ContestSignUp> ContestSignUps { get; set; }
        
        // Certificate
        public DbSet<Certificate.Certificate> Certificates { get; set; }
        
        public acmManagerDbContext(DbContextOptions<acmManagerDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
