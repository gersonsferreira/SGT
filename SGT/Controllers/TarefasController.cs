using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGT.Data;
using SGT.Models;
using SGT.Models.Domain;

namespace SGT.Controllers
{
    public class TarefasController : Controller
    {
        private readonly MVCDbContext mVCDbContext1;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TarefasController(MVCDbContext mVCDbContext, IHttpContextAccessor httpContextAccessor)
        {
            mVCDbContext1 = mVCDbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tarefas = await mVCDbContext1.Tarefas.ToListAsync();
            return View(tarefas);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> Add(AddTarefasViewModel model)
        {
            if (ModelState.IsValid)
            {
                string usuarioId = httpContextAccessor.HttpContext.Session.GetString("_UsuarioId");
                var usuario = await mVCDbContext1.Usuario.FirstOrDefaultAsync(x => x.Id.ToString() == usuarioId);
                var status = await mVCDbContext1.Status.FirstOrDefaultAsync(x => x.Descricao == TipoStatusViewModel.Pendente);
                var tarefa = new Tarefas
                {
                    Id = Guid.NewGuid(),
                    Descricao = model.Descricao,
                    DataCriacao = DateTime.Now,
                    IdUsuario = usuario.Id,
                    IdStatus = status.Id
                };
                await mVCDbContext1.Tarefas.AddAsync(tarefa);
                await mVCDbContext1.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task <IActionResult> View(Guid id)
        {
            var tarefa = await mVCDbContext1.Tarefas.FirstOrDefaultAsync(x => x.Id == id);

            if (tarefa != null)
            {
                var viewModel = new UpdateTarefasViewModel
                {
                    Id = tarefa.Id,
                    Descricao = tarefa.Descricao,
                    DataCriacao = tarefa.DataCriacao,
                    DataConclusao = tarefa.DataConclusao,
                    IdUsuario = tarefa.IdUsuario,
                    IdStatus = tarefa.IdStatus
                };
                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateTarefasViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tarefa = await mVCDbContext1.Tarefas.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (tarefa != null)
                {
                    tarefa.Descricao = model.Descricao;
                    tarefa.IdUsuario = model.IdUsuario;
                    tarefa.IdStatus = model.IdStatus;

                    await mVCDbContext1.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateTarefasViewModel model)
        {
            var tarefa = await mVCDbContext1.Tarefas.FindAsync(model.Id);

            if (tarefa != null)
            {
                mVCDbContext1.Tarefas.Remove(tarefa);
                await mVCDbContext1.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewEmProgresso(UpdateTarefasViewModel model)
        {
            var tarefa = await mVCDbContext1.Tarefas.FindAsync(model.Id);

            if (tarefa != null)
            {
                var status = await mVCDbContext1.Status.FirstOrDefaultAsync(x => x.Descricao == TipoStatusViewModel.EmProgresso);
                tarefa.IdStatus = status.Id;
                tarefa.DataConclusao = DateTime.Now;

                await mVCDbContext1.SaveChangesAsync();
            }

            return RedirectToAction("View");
        }

        [HttpGet]
        public async Task<IActionResult> ViewConcluir(UpdateTarefasViewModel model)
        {
            var tarefa = await mVCDbContext1.Tarefas.FindAsync(model.Id);

            if (tarefa != null)
            {
                var status = await mVCDbContext1.Status.FirstOrDefaultAsync(x => x.Descricao == TipoStatusViewModel.Concluido);
                tarefa.IdStatus = status.Id;

                await mVCDbContext1.SaveChangesAsync();
            }

            return RedirectToAction("View");
        }
    }
}
