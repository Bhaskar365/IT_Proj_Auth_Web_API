using IT_Proj_Db_Regis.Models;
using Microsoft.EntityFrameworkCore;

namespace IT_Proj_Db_Regis.DbContextClass
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options):base(options) { }


        public DbSet<User> User { get; set; } 


    }
}
