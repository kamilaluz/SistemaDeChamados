namespace Tickets.Models
{
    public class ChamadoComentarios
    {
        public Chamado Chamado { get; set; }
        public Comentario Comentario { get; set; }
        public List<Comentario> ListaComentarios { get; set; }
    }
}
