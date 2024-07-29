namespace SGT.Models.Domain
{
    public class Tarefas
    {
        public Guid Id {get;set;}
        public string Descricao {get; set;}
        public DateTime DataCriacao {get; set;}
        public DateTime DataConclusao {get; set;}
        public Guid IdUsuario {get; set;}
        public Guid IdStatus {get; set;}
    }
}
