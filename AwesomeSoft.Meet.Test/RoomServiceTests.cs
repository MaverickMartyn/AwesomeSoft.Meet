using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AwesomeSoft.Meet.Test
{
    public class RoomServiceTests
    {
        private ServiceProvider serviceProvider;
        private RoomService roomService;
        private Meeting meeting1;

        private readonly Room room1 = new Room()
        {
            Name = "Meeting Room 1"
        };

        private readonly Room room2 = new Room()
        {
            Name = "Office"
        };

        private readonly User user1 = new User()
        {
            Name = "JustAUser"
        };

        #region Setup, Cleanup and Helpers.
        private void Setup()
        {
            IServiceCollection services = new ServiceCollection();

            var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
            keepAliveConnection.Open();

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(keepAliveConnection));
            serviceProvider = services.BuildServiceProvider();
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            context.Users.Add(user1);
            context.Rooms.AddRange(room1, room2);
            context.SaveChanges();

            meeting1 = context.Meetings.Add(new Meeting()
            {
                Title = "Meeting 1",
                Description = "This is a meeting.",
                StartTime = new DateTime(2021, 4, 1, 13, 30, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 30, 0),
                Owner = user1,
                Room = room2
            }).Entity;
            context.SaveChanges();

            roomService = new RoomService(context);
        }

        private void Cleanup()
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.CloseConnectionAsync();
        }
        #endregion

        [Fact]
        public void GetUnoccupiedRoomsTest()
        {
            Setup();

            // Gets all rooms that are unoccupied during the given datetime range.
            List<Room> rooms1 = roomService.GetRooms(new DateTime(2021, 4, 1, 13, 30, 0), new DateTime(2021, 4, 1, 15, 00, 0));
            
            // Does the same thing but ignores a specific meeting, for use when editing it.
            List<Room> rooms2 = roomService.GetRooms(new DateTime(2021, 4, 1, 13, 30, 0), new DateTime(2021, 4, 1, 15, 00, 0), meeting1.Id);

            Assert.DoesNotContain(room2, rooms1);
            Assert.Equal(roomService.GetRooms(), rooms2); // Assert whether second list contains all rooms.

            Cleanup();
        }
    }
}
