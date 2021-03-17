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
        public List<Meeting> GetMeetings(User user)
        {
            return meetings.Where(m => m.Owner.Id == user.Id || m.Participants.Any(p => p.Id == user.Id)).ToList();
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
        /// Returns a list of <see cref="Conflict"/> objects representing <see cref="Meeting"/>s of the current user with overlapping time tables.
        /// </summary>
        /// <returns>List of <see cref="Conflict"/> objects.</returns>
        public List<Conflict> GetConflicts(User user)
        {
            List<Conflict> conflicts = new List<Conflict>();
            foreach (Meeting meeting in GetMeetings(user))
            {
                if (!conflicts.Any(c => c.Meeting.Id == meeting.Id || c.Conflicts.Any(c => c.Id == meeting.Id)))
                {
                    var conflictingMeetings = meetings.Where(m => m.Id != meeting.Id && m.Owner.Id == user.Id && ((m.StartTime >= meeting.StartTime && m.StartTime <= meeting.EndTime) || (m.EndTime <= meeting.EndTime && m.StartTime >= meeting.StartTime)));
                    if (conflictingMeetings.Any())
                    {
                        conflicts.Add(new Models.Conflict(meeting, conflictingMeetings.ToList()));
                    }
                }
            }
            return conflicts;
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
