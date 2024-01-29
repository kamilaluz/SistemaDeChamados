using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tickets.Models
{
    [Table("chamados")]
    public class Chamado
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        [Required]
        [ForeignKey("FuncionarioId")]
        public int FuncionarioId {  get; set; }
        public Usuario Funcionario { get; set; }
        [Required]
        [ForeignKey("AtendenteId")]

        public int AtendenteId { get; set; }
        public Usuario Atendente { get; set; }
       
    }


}
