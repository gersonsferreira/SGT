using Microsoft.AspNetCore.Mvc;
using SGT.Data;
using SGT.Models;
using Microsoft.EntityFrameworkCore;
using SGT.Models.Domain;

namespace SGT.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly MVCDbContext mVCDbContext1;

        public UsuarioController(MVCDbContext mVCDbContext)
        {
            mVCDbContext1 = mVCDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuario = await mVCDbContext1.Usuario.ToListAsync();
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> Add(AddUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = model.Nome,
                    Email = model.Email,
                    Senha = model.Senha,
                    Admin = model.Admin
                };
                await mVCDbContext1.Usuario.AddAsync(usuario);
                await mVCDbContext1.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var Usuario = await mVCDbContext1.Usuario.FirstOrDefaultAsync(x => x.Id == id);

            if (Usuario != null)
            {
                var viewModel = new UpdateUsuarioViewModel
                {
                    Id = Usuario.Id,
                    Nome = Usuario.Nome,
                    Email = Usuario.Email,
                    Senha = Usuario.Senha,
                    Admin = Usuario.Admin
                };
                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await mVCDbContext1.Usuario.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (usuario != null)
                {
                    usuario.Nome = model.Nome;
                    usuario.Email = model.Email;
                    usuario.Senha = model.Senha;
                    usuario.Admin = model.Admin;

                    await mVCDbContext1.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateUsuarioViewModel model)
        {
            var Usuario = await mVCDbContext1.Usuario.FindAsync(model.Id);

            if (Usuario != null)
            {
                mVCDbContext1.Usuario.Remove(Usuario);
                await mVCDbContext1.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ObterUsuarioPorEmail(string email)
        {
            var usuario = mVCDbContext1.Usuario.FirstOrDefaultAsync(x => x.Email == email);
            return RedirectToAction("Index");
        }
    }
}