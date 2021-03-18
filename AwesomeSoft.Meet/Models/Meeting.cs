using System;
using System.Collections.Generic;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// A calendar entry describing a scheduled Meeting.
    /// </summary>
    public class Meeting
    {
        public uint Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public User Owner { get; set; }

        public List<User> Participants { get; set; } = new List<User>();

        public Room Room { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IEnumerable<uint> ConflictingIds { get; set; }
    }
}