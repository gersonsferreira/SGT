namespace SGT.Models.Domain
{
    public class Usuario
    {
        public Guid Id {get;set;}
        public string Nome {get; set;}
        public string Email {get; set;}
        public string Senha {get; set;}
        public bool Admin {get; set;}
    }
}
