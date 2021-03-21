using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace AwesomeSoft.Meet.Test
{
    public class MeetingServiceTests
    {
        private MeetingService _meetingService;
        ServiceProvider serviceProvider;

        private readonly User user1 = new User()
        {
            Name = "JustAUser"
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
            context.SaveChanges();

            _meetingService = new MeetingService(context);
        }

        private void Cleanup()
        {
            ApplicationDbContext context = serviceProvider.GetService<ApplicationDbContext>();
            context.Database.CloseConnectionAsync();
        }
        #endregion

        /// <summary>
        /// Tests the conflict checking logic.
        /// </summary>
        [Fact]
        public void CheckForConflictsTest()
        {
            Setup();

            Meeting meeting1 = _meetingService.Add(new Meeting()
            {
                Title = "Meeting 1",
                Description = "This is a meeting.",
                StartTime = new DateTime(2021, 4, 1, 13, 30, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 30, 0),
                Owner = user1
            });

            Meeting meeting2 = _meetingService.Add(new Meeting()
            {
                Title = "Meeting 2",
                Description = "This is a meeting.",
                StartTime = new DateTime(2021, 4, 1, 13, 0, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 0, 0),
                Owner = user1
            });

            List<Meeting> meetings = _meetingService.CheckForConflicts(new List<Meeting>() {
                meeting1,
                meeting2
            });

            Assert.Contains(meetings[1].Id, meetings[0].ConflictingIds);
            Assert.Contains(meetings[0].Id, meetings[1].ConflictingIds);

            Cleanup();
        }

        /// <summary>
        /// Tests getting meetings within a given DateTime range.
        /// </summary>
        [Fact]
        public void GetMeetingsInTimeRangeTest()
        {
            Setup();

            Meeting meeting1 = _meetingService.Add(new Meeting()
            {
                Title = "Meeting 1",
                Description = "This is a meeting set on 1. April 2021 1:30PM until 2:30 PM.",
                StartTime = new DateTime(2021, 4, 1, 13, 30, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 30, 0),
                Owner = user1
            });

            // Starts within the range, but ends after. Should be included.
            Meeting meeting2 = _meetingService.Add(new Meeting()
            {
                Title = "Meeting 2",
                Description = "This is a meeting set on 11. June 2021 1PM until 2 PM.",
                StartTime = new DateTime(2021, 6, 11, 13, 0, 0),
                EndTime = new DateTime(2021, 6, 11, 14, 0, 0),
                Owner = user1
            });

            Meeting meeting3 = _meetingService.Add(new Meeting()
            {
                Title = "Meeting 3",
                Description = "This is a meeting set on 12. June 2021 2PM until 3 PM.",
                StartTime = new DateTime(2021, 6, 12, 14, 0, 0),
                EndTime = new DateTime(2021, 6, 12, 15, 0, 0),
                Owner = user1
            });

            List<Meeting> meetings = _meetingService.GetMeetings(user1, new DateTime(2021, 4, 1, 13, 30, 0), new DateTime(2021, 6, 11, 13, 30, 0));

            Assert.DoesNotContain(meeting3, meetings);
            Assert.Contains(meeting1, meetings);
            Assert.Contains(meeting2, meetings);

            Cleanup();
        }
    }
}
