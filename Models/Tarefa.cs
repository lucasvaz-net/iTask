using Microsoft.AspNetCore.Mvc.Rendering;

namespace iTask.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataVencimento { get; set; }
        public int Prioridade { get; set; }
        public string CategoriaId { get; set; }
        public string StatusId { get; set; }
        public string UsuarioId { get; set; }
    }
}
