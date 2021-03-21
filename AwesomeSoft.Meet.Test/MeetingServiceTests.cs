using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace AwesomeSoft.Meet.Test
{
    public class MeetingServiceTests
    {
        private MeetingService _meetingService;
        private readonly User User1 = new User()
        {
            Id = 30,
            Name = "JustAUser"
        };

        /// <summary>
        /// Prepares test environment.
        /// </summary>
        private void Setup()
        {
            _meetingService = new MeetingService();
        }

        /// <summary>
        /// Tests the conflict checking logic.
        /// </summary>
        [Fact]
        public void CheckForConflictsTest()
        {
            Setup();

            Meeting meeting1 = _meetingService.Add(new Meeting() {
                Title = "Meeting 1",
                Description = "This is a meeting.",
                StartTime = new DateTime(2021, 4, 1, 13, 30, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 30, 0),
                Owner = User1
            });

            Meeting meeting2 = _meetingService.Add(new Meeting()
            {
                Title = "Meeting 2",
                Description = "This is a meeting.",
                StartTime = new DateTime(2021, 4, 1, 13, 0, 0),
                EndTime = new DateTime(2021, 4, 1, 14, 0, 0),
                Owner = User1
            });

            List<Meeting> meetings = _meetingService.CheckForConflicts(new List<Meeting>() {
                meeting1,
                meeting2
            });

            Assert.Contains(meetings[1].Id, meetings[0].ConflictingIds);
            Assert.Contains(meetings[0].Id, meetings[1].ConflictingIds);
        }
    }
}
