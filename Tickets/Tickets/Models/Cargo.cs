using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets.Models
{
    [Table("cargos")]
    public class Cargo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

    }
}
