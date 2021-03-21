using System.ComponentModel.DataAnnotations;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// Represents a meeting room.
    /// </summary>
    public class Room
    {
        public uint Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
