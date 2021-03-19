using AwesomeSoft.Meet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeSoft.Meet.Helpers;

namespace AwesomeSoft.Meet.Services
{
    public class RoomService
    {
        private readonly List<Room> rooms = DummyData.Instance.Rooms;
        private readonly Random rand = new Random();
        private readonly MeetingService _meetingService;

        public RoomService(MeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        /// <summary>
        /// Returns all rooms.
        /// </summary>
        /// <returns>A generic list of <see cref="Room"/>s.</returns>
        public List<Room> GetRooms()
        {
            return rooms;
        }

        /// <summary>
        /// Returns all rooms.
        /// </summary>
        /// <param name="user">The <see cref="User"/>.</param>
        /// <param name="startTime">The start (inclusive) of the date range.</param>
        /// <param name="endTime">The end (inclusive) of the date range.</param>
        /// <returns>A generic list of <see cref="Room"/>s.</returns>
        public List<Room> GetRooms(User user, DateTime startTime, DateTime endTime)
        {
            var meetings = _meetingService.GetMeetings(user, startTime, endTime);
            return rooms.Where(r => !meetings.Any(m => m.Room.Id == r.Id)).ToList();
        }

        /// <summary>
        /// Returns all rooms.
        /// </summary>
        /// <returns>A generic list of <see cref="Room"/>s.</returns>
        public Room GetRoomById(uint id)
        {
            return rooms.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Adds a <see cref="Room"/> and returns it with its new ID.
        /// </summary>
        /// <returns>The new <see cref="Room"/> with its assigned ID.</returns>
        public Room Add(Room room)
        {
            // Handle adding to data store using DBContext or similar.
            room.Id = (uint)rand.Next(1, int.MaxValue); // Sets a dummy ID since no database is present.
            rooms.Add(room);
            return room;
        }

        /// <summary>
        /// Updates a room in the data store.
        /// </summary>
        /// <returns>The updated <see cref="Room"/> freshly grabbed from the data store.</returns>
        public Room Update(Room room)
        {
            // Save data to database.
            rooms[rooms.IndexOf(GetRoomById(room.Id))] = room;
            return GetRoomById(room.Id);
        }

        /// <summary>
        /// Deletes a given room from the data store.
        /// </summary>
        public void Delete(Room room)
        {
            rooms.Remove(room);
        }
    }
}