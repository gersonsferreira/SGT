using System.ComponentModel.DataAnnotations;

namespace SGT.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a senha")]
        public string Senha { get; set; }

    }
}
