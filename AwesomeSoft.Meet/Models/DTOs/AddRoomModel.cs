using System.ComponentModel.DataAnnotations;

namespace AwesomeSoft.Meet.Models.DTOs
{
    public class AddRoomModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}