using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Room> Rooms { get; set; }
    }
}
