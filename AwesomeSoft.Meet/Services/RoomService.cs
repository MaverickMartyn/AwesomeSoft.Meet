using AwesomeSoft.Meet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeSoft.Meet.Helpers;

namespace AwesomeSoft.Meet.Services
{
    public class RoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Methods
        /// <summary>
        /// Returns all rooms.
        /// </summary>
        /// <returns>A generic list of <see cref="Room"/>s.</returns>
        public List<Room> GetRooms()
        {
            return _context.Rooms.ToList();
        }

        /// <summary>
        /// Returns all available rooms.
        /// </summary>
        /// <param name="startTime">The start (inclusive) of the date range.</param>
        /// <param name="endTime">The end (inclusive) of the date range.</param>
        /// <param name="ignoredId">Id of a meeting to ignore. (Used when editing a meeting to avoid the current room showing as occupied).</param>
        /// <returns>A generic list of <see cref="Room"/>s.</returns>
        public List<Room> GetRooms(DateTime startTime, DateTime endTime, uint ignoredId = 0)
        {
            var occupiedRoomIds = _context.Meetings.Where(m => m.Id != ignoredId && ((m.StartTime >= startTime && m.StartTime <= endTime) || (m.EndTime >= startTime && m.EndTime <= endTime))).Select(m => m.Room.Id);
            return _context.Rooms.Where(r => !occupiedRoomIds.Any(id => id == r.Id)).ToList();
        }

        /// <summary>
        /// Returns all rooms.
        /// </summary>
        /// <returns>A generic list of <see cref="Room"/>s.</returns>
        public Room GetRoomById(uint id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Adds a <see cref="Room"/> and returns it with its new ID.
        /// </summary>
        /// <returns>The new <see cref="Room"/> with its assigned ID.</returns>
        public Room Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return room;
        }

        /// <summary>
        /// Updates a room in the data store.
        /// </summary>
        /// <returns>The updated <see cref="Room"/> freshly grabbed from the data store.</returns>
        public Room Update(Room room)
        {
            // Save data to database.
            _context.Rooms.Update(room);
            _context.SaveChanges();
            return room;
        }

        /// <summary>
        /// Deletes a given room from the data store.
        /// </summary>
        public void Delete(Room room)
        {
            _context.Rooms.Remove(room);
            _context.SaveChanges();
        }
        #endregion
    }
}