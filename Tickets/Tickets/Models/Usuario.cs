using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets.Models
{
    public class Usuario : IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public DateOnly Nascimento { get; set; }
        [Required]
        public int CargoId {  get; set; }
        public virtual Cargo Cargo { get; set; }

    }
}
