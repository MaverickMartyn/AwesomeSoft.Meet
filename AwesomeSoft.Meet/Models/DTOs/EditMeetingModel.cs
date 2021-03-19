using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwesomeSoft.Meet.Models.DTOs
{
    /// <summary>
    /// Represents a <see cref="Meeting"/> while editing it.
    /// </summary>
    public class EditMeetingModel
    {
        [Required]
        public uint Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(typeof(uint), "1", "4294967295")] // Not ideal. Try to refactor later.
        public uint RoomId { get; set; }

        public IEnumerable<uint> ParticipantIds { get; set; }
    }
}