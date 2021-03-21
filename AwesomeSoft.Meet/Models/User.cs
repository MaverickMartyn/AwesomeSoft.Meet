using System.ComponentModel.DataAnnotations;

namespace AwesomeSoft.Meet.Models
{
    /// <summary>
    /// A system user.
    /// </summary>
    public class User
    {
        public uint Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
