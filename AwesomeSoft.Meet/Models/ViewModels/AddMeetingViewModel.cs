using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSoft.Meet.Models.ViewModels
{
    /// <summary>
    /// Represents a <see cref="Meeting"/> while creating it.
    /// </summary>
    public class AddMeetingViewModel
    {
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
        public uint RoomId { get; set; }

        public IEnumerable<Participant> Participants { get; set; }
    }
}
