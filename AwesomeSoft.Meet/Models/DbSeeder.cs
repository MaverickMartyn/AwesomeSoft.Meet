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
                    Title = "SCRUM Meeting",
                    Description = "Get the workers working.\nWhip them if necessary.",
                    // Explicit OrderBy Id at all FirstOrDefault calls since SQLite requires it.
                    Owner = context.Users.OrderBy(u => u.Id).First(u => u.Name == "User1"), // User 1 created this and is the only participant.
                    StartTime = DateTime.Today.Add(new TimeSpan(12, 30, 0)).AddDays(1),
                    EndTime = DateTime.Today.Add(new TimeSpan(14, 30, 0)).AddDays(1),
                    Room = context.Rooms.OrderBy(r => r.Id).First(),
                    Participants = new List<User>()
                },
                new Meeting() {
                    Title = "BestBusiness Aps meeting",
                    Description = "Contract talks with BestBusiness Aps.\nRemember cookies and subliminal messaging.\nMake sure User2 participates",
                    Owner = context.Users.OrderBy(u => u.Id).First(u => u.Name == "User1"), // User 1 created this.
                    StartTime = DateTime.Today.Add(new TimeSpan(13, 30, 0)),
                    EndTime = DateTime.Today.Add(new TimeSpan(15, 30, 0)),
                    Participants = new List<User>()
                    {
                        context.Users.OrderBy(u => u.Id).First(u => u.Name == "User2"), // User 2 participates.
                    },
                    Room = context.Rooms.OrderBy(r => r.Id).First()
                },
                new Meeting() {
                    Title = "Conflict resolution 101",
                    Description = "This is supposed to conflict with the BestBusiness Aps meeting.\nIf it doesn't, something is wrong.",
                    Owner = context.Users.OrderBy(u => u.Id).First(u => u.Name == "User2"), // User 2 created this.
                    StartTime = DateTime.Today.Add(new TimeSpan(12, 00, 0)),
                    EndTime = DateTime.Today.Add(new TimeSpan(14, 00, 0)),
                    Participants = new List<User>()
                    {
                        context.Users.OrderBy(u => u.Id).First(u => u.Name == "User1"), // User 1 participates.
                    },
                    Room = context.Rooms.OrderBy(r => r.Id).First()
                },
                new Meeting() {
                    Title = "Emergency Meeting",
                    Description = "This is important.\nRemember to join me User1!",
                    Owner = context.Users.OrderBy(u => u.Id).First(u => u.Name == "User2"), // User 2 created this.
                    StartTime = DateTime.Today.Add(new TimeSpan(12, 00, 0)),
                    EndTime = DateTime.Today.Add(new TimeSpan(14, 00, 0)),
                    Participants = new List<User>()
                    {
                        context.Users.OrderBy(u => u.Id).First(u => u.Name == "User1"), // User 1 participates.
                    },
                    Room = context.Rooms.OrderBy(r => r.Id).First()
                }
            };
            context.Meetings.AddRange(meetings);
            context.SaveChanges();
        }
    }
}
