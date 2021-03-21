using AwesomeSoft.Meet.Controllers;
using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class MeetingsControllerTests
    {
        private ServiceProvider serviceProvider;
        private MeetingsController _meetingsController;
        private Meeting meeting1;
        private readonly User user1 = new User()
        {
            Name = "JustAUser"
        };

        private readonly Room room1 = new Room()
        {
            Name = "Meeting Room 1"
        };

        #region Setup, Cleanup and Helpers.
        /// <summary>
        /// Prepares test environment.
        /// </summary>
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
            context.Rooms.Add(room1);
            context.SaveChanges();

            meeting1 = context.Meetings.Add(new Meeting()
            {
                Title = "Meeting 1",
                Description = "This is a meeting.",
                StartTime = new DateTime(2021, 4, 1, 13, 30, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 30, 0),
                Owner = user1,
                Room = room1
            }).Entity;
            context.SaveChanges();

            _meetingsController = new MeetingsController(new UserService(context), new MeetingService(context), new RoomService(context));
        }

        private void Cleanup()
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.CloseConnectionAsync();
        }
        #endregion

        [Fact]
        public void PutMeetingTest()
        {
            Setup();

            // Setup fake HttpContext and user session.
            _meetingsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    Items = new Dictionary<object, object>()
                    {
                        { "User", user1 }
                    }
                }
            };

            var result = _meetingsController.Put(meeting1.Id, new Models.DTOs.EditMeetingModel()
            {
                Id = meeting1.Id,
                Title = "Meeting 1.2",
                Description = meeting1.Description,
                ParticipantIds = new List<uint>(),
                RoomId = meeting1.Room.Id,
                StartTime = meeting1.StartTime.AddDays(2),
                EndTime = meeting1.EndTime.AddDays(2)
            });

            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<Meeting>(actionResult.Value);
            var returnedMeeting = actionResult.Value as Meeting;
            Assert.NotNull(returnedMeeting);
            Assert.Equal("Meeting 1.2", returnedMeeting.Title);
            Assert.Equal(new DateTime(2021, 4, 3, 13, 30, 0), returnedMeeting.StartTime);
            Assert.Equal(new DateTime(2021, 4, 3, 14, 30, 0), returnedMeeting.EndTime);

            Cleanup();
        }
    }
}
