using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGT.Data;
using SGT.Models;
using SGT.Models.Domain;

namespace SGT.Controllers
{
    public class LoginController : Controller
    {

        private readonly MVCDbContext mVCDbContext1;

        public LoginController(MVCDbContext mVCDbContext)
        {
            mVCDbContext1 = mVCDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Entrar(LoginViewModel loginViewModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(loginViewModel.Email.Contains("adm") )
                    {
                        Usuario usuario = await mVCDbContext1.Usuario.FirstOrDefaultAsync(x => x.Email == loginViewModel.Email);
                        if(usuario != null)
                        {
                            if(usuario.Senha == loginViewModel.Senha)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                        }
                    }
                    TempData["MensagemErro"] = $"Usuário e/ou Senha inválidos. Por favor tente novamente.";
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não conseguimos realizar seu login, detalhe do erro: {ex.Message}";

                return RedirectToAction("Index");
            }
        }
    }
}
