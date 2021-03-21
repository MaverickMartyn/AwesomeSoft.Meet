using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// A calendar entry describing a scheduled Meeting.
    /// </summary>
    public class Meeting
    {
        public uint Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        public User Owner { get; set; }

        public List<User> Participants { get; set; } = new List<User>();

        [Required]
        public Room Room { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [NotMapped]
        public IEnumerable<uint> ConflictingIds { get; set; }
    }
}