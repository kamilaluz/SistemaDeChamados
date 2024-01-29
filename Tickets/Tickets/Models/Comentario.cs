using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets.Models
{
    [Table("comentarios")]
    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [ForeignKey("AutorId")]
        public int AutorId { get; set; }
        public virtual Usuario Autor { get; set; }
        [Required]
        public DateTime Data { get; set; }
        public int ChamadoId { get; set; }
        public Chamado Chamado { get; set; }

    }
}
