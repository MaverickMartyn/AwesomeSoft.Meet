using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// Database seeding method.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task AddTestDataAsync(ApplicationDbContext context)
        {
            var users = new List<User>()
            {
                new User()
                {
                    Name = "User1"
                },
                new User()
                {
                    Name = "User2"
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var rooms = new List<Room>()
            {
                new Room()
                {
                    Name = "Meeting Room 1"
                },
                new Room()
                {
                    Name = "Meeting Room 2"
                },
                new Room()
                {
                    Name = "Large Meeting Room 1"
                },
                new Room()
                {
                    Name = "Office 1"
                }
            };
            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            var meetings = new List<Meeting>()
            {
                new Meeting() {
                    Id = 1,
                    Title = "Test meeting 1",
                    Description = "This is just a test meeting.\nNothing to see here.",
                    // Explicit OrderBy Id at all FirstOrDefault calls since SQLite requires it.
                    Owner = await context.Users.OrderBy(u => u.Id).FirstOrDefaultAsync(),
                    Room = await context.Rooms.OrderBy(r => r.Id).FirstOrDefaultAsync()
                },
                new Meeting() {
                    Id = 2,
                    Title = "Test meeting 2",
                    Description = "This is just a another test meeting.\nNothing to see here.",
                    Owner = await context.Users.OrderBy(u => u.Id).LastOrDefaultAsync(),
                    Participants = new List<User>()
                    {
                        await context.Users.OrderBy(u => u.Id).FirstOrDefaultAsync(),
                    },
                    Room = await context.Rooms.OrderBy(r => r.Id).FirstOrDefaultAsync()
                }
            };
            context.Meetings.AddRange(meetings);
            context.SaveChanges();
        }
    }
}
