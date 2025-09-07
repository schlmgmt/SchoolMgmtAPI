using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolMgmtAPI.Models.DbModel;

namespace SchoolMgmtAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<School> School {  get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<EmailTemplates> EmailTemplates { get; set; }
    }
}
