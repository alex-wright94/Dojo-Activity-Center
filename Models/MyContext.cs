using Microsoft.EntityFrameworkCore;
using NewBeltExam.Models;

namespace NewBeltExam.Models {
    public class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Party> Partys {get;set;}
        public DbSet<Join> Joins {get;set;}
    }
}