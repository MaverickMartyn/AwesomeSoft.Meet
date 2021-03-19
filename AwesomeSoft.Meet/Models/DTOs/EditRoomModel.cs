using System.ComponentModel.DataAnnotations;

namespace AwesomeSoft.Meet.Models.DTOs
{
    public class EditRoomModel
    {
        [Required]
        public uint Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}