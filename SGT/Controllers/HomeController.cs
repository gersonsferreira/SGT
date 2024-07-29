using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGT.Data;
using SGT.Models;
using SGT.Models.Domain;
using System.Diagnostics;

namespace SGT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MVCDbContext mVCDbContext1;
        public const string SessionUsuarioId = "_UsuarioId";

        public HomeController(MVCDbContext mVCDbContext)
        {
            mVCDbContext1 = mVCDbContext;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {
            var usuarioBanco = mVCDbContext1.Usuario.FirstOrDefault(x => x.Email == usuario.Email && x.Senha == usuario.Senha);
            if (usuarioBanco != null)
            {
                HttpContext.Session.SetString(SessionUsuarioId, usuarioBanco.Id.ToString());
                return RedirectToAction("Dashboard");
            }
            return View("../Home/Index");
        }

        public IActionResult Dashboard()
        {
            Status statusPendente = mVCDbContext1.Status.FirstOrDefault(x => x.Descricao == TipoStatusViewModel.Pendente);
            Status statusEmProgresso = mVCDbContext1.Status.FirstOrDefault(x => x.Descricao == TipoStatusViewModel.EmProgresso);
            Status statusConcluido = mVCDbContext1.Status.FirstOrDefault(x => x.Descricao == TipoStatusViewModel.Concluido);

            var viewModel = new DashboardViewModel
            {
                TotalTarefas = mVCDbContext1.Tarefas.Count(),
                TarefasConcluidas = mVCDbContext1.Tarefas.Count(x => x.IdStatus == statusConcluido.Id),
                TarefasPendentes = mVCDbContext1.Tarefas.Count(x => x.IdStatus == statusPendente.Id),
                TotalEmProgresso = mVCDbContext1.Tarefas.Count(x => x.IdStatus == statusEmProgresso.Id)
            };
            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
