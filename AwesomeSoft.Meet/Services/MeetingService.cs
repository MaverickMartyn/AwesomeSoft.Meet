using AwesomeSoft.Meet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeSoft.Meet.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AwesomeSoft.Meet.Services
{
    public class MeetingService
    {
        private readonly ApplicationDbContext _context;

        public MeetingService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Methods
        /// <summary>
        /// Returns all <see cref="Meeting"/>s owned by or attended by this user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A List of all <see cref="Meeting"/>s attended by this user.</returns>
        public IQueryable<Meeting> GetMeetings(User user)
        {
            return _context.Meetings.Include(m => m.Room).Include(m => m.Participants).Where(m => m.Owner.Id == user.Id || m.Participants.Any(p => p.Id == user.Id));
        }

        /// <summary>
        /// Returns all <see cref="Meeting"/>s owned by or attended by this user in a specified timeframe.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="startTime">Start of the time range.</param>
        /// <param name="endTime">End of the time range.</param>
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
            return _context.Meetings.ToList();
        }

        /// <summary>
        /// Checks a list of <see cref="Meeting"/>s for conflicts, and sets the ConflictingIds property for each <see cref="Meeting"/> that overlas another meetings scheduled times.
        /// </summary>
        /// <returns>List of <see cref="Meeting"/>s with ConflictingIds set.</returns>
        public List<Meeting> CheckForConflicts(List<Meeting> meetings)
        {
            foreach (Meeting meeting in meetings)
            {
                var conflictingMeetings = meetings.Where(m => m.Id != meeting.Id && ((m.StartTime >= meeting.StartTime && m.StartTime <= meeting.EndTime) || (m.EndTime <= meeting.EndTime && m.EndTime >= meeting.StartTime)));
                if (conflictingMeetings.Any())
                {
                    meeting.ConflictingIds = conflictingMeetings.Select(cm => cm.Id);
                }
            }
            return meetings;
        }

        /// <summary>
        /// Checks single <see cref="Meeting"/> for conflicts, and sets the ConflictingIds property if it overlaps another meetings scheduled times.
        /// </summary>
        /// <returns>List of <see cref="Meeting"/>s with ConflictingIds set.</returns>
        public Meeting CheckForConflicts(Meeting meeting)
        {
            return CheckForConflicts(GetMeetings(meeting.Owner, meeting.StartTime, meeting.EndTime)).FirstOrDefault(m => m.Id == meeting.Id);
        }

        /// <summary>
        /// Adds a meeting to the store.
        /// </summary>
        /// <param name="meeting">The finished <see cref="Meeting"/> object.</param>
        /// <returns>Returns the given object, with an added ID, if successful.</returns>
        public Meeting Add(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
            var newMeeting = _context.Meetings.FirstOrDefault(m => m.Id == meeting.Id);
            return meeting;
        }

        /// <summary>
        /// Returns a specific <see cref="Meeting"/> by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The requested <see cref="Meeting"/> or null.</returns>
        public Meeting GetMeetingById(uint id)
        {
            return _context.Meetings.FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Overrides a <see cref="Meeting"/> with a modified version.
        /// </summary>
        /// <param name="meeting">The modified <see cref="Meeting"/></param>
        /// <returns>Returns the meeting, as saved in the data store.</returns>
        public Meeting Update(Meeting meeting)
        {
            // Save data to database.
            _context.Meetings.Update(meeting);
            _context.SaveChanges();
            return meeting;
        }

        /// <summary>
        /// Deletes a <see cref="Meeting"/>.
        /// </summary>
        /// <param name="meeting">The <see cref="Meeting"/> to delete.</param>
        public void Delete(Meeting meeting)
        {
            _context.Meetings.Remove(meeting);
            _context.SaveChanges();
        }
        #endregion
    }
}
