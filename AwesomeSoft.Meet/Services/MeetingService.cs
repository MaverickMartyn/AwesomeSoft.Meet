using AwesomeSoft.Meet.Models;
using AwesomeSoft.Meet.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Services
{
    public class MeetingService
    {
        // Dummy data container
        private List<Meeting> meetings = DummyData.Instance.Meetings;
        private Random rand = new Random();

        #region Methods
        /// <summary>
        /// Returns all <see cref="Meeting"/>s owned by or attended by this user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A List of all <see cref="Meeting"/>s attended by this user.</returns>
        public List<Meeting> GetMeetings(User user)
        {
            return meetings.Where(m => m.Owner.Id == user.Id || m.Participants.Any(p => p.Id == user.Id)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="Meeting"/>s owned by or attended by this user in a specified timeframe..
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A List of all <see cref="Meeting"/>s attended by this user in the given timeframe.</returns>
        public List<Meeting> GetMeetings(User user, DateTime startTime, DateTime endTime)
        {
            return GetMeetings(user).Where(m => (m.StartTime >= startTime && m.StartTime <= endTime) || (m.EndTime >= startTime && m.EndTime <= endTime)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="Meeting"/>s.
        /// </summary>
        /// <returns>A collection of <see cref="Meeting"/>s.</returns>
        public List<Meeting> GetAllMeetings()
        {
            return meetings.ToList();
        }

        /// <summary>
        /// Checks a list of <see cref="Meeting"/>s for conflicts, and sets the ConflictingIds property for each <see cref="Meeting"/> that overlas another meetings scheduled times.
        /// </summary>
        /// <returns>List of <see cref="Meeting"/>s with ConflictingIds set.</returns>
        public List<Meeting> CheckForConflicts(List<Meeting> meetings)
        {
            foreach (Meeting meeting in meetings)
            {
                var conflictingMeetings = meetings.Where(m => m.Id != meeting.Id && ((m.StartTime >= meeting.StartTime && m.StartTime <= meeting.EndTime) || (m.EndTime <= meeting.EndTime && m.StartTime >= meeting.StartTime)));
                if (conflictingMeetings.Any())
                {
                    meeting.ConflictingIds = conflictingMeetings.Select(cm => cm.Id);
                }
            }
            return meetings;
        }

        /// <summary>
        /// Adds a meeting to the store.
        /// </summary>
        /// <param name="meeting">The finished <see cref="Meeting"/> object.</param>
        /// <returns>Returns the given object, with an added ID, if successful.</returns>
        public Meeting Add(Meeting meeting)
        {
            // Handle adding to data store using DBContext or similar.
            meeting.Id = (uint)rand.Next(1, int.MaxValue); // Sets a dummy ID since no database is present.
            meetings.Add(meeting);
            return meeting;
        }

        /// <summary>
        /// Returns a specific <see cref="Meeting"/> by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The requested <see cref="Meeting"/> or null.</returns>
        public Meeting GetMeetingById(uint id)
        {
            return meetings.FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Overrides a <see cref="Meeting"/> with a modified version.
        /// </summary>
        /// <param name="meeting">The modified <see cref="Meeting"/></param>
        /// <returns>Returns the meeting, as saved in the data store.</returns>
        public Meeting Update(Meeting meeting)
        {
            // Save data to database.
            meetings[meetings.IndexOf(GetMeetingById(meeting.Id))] = meeting;
            return GetMeetingById(meeting.Id);
        }

        /// <summary>
        /// Deletes a <see cref="Meeting"/>.
        /// </summary>
        /// <param name="meeting">The <see cref="Meeting"/> to delete.</param>
        public void Delete(Meeting meeting)
        {
            meetings.Remove(meeting);
        }
        #endregion
    }
}
