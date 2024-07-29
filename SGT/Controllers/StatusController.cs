using Microsoft.AspNetCore.Mvc;
using SGT.Data;
using SGT.Models.Domain;
using SGT.Models;
using Microsoft.EntityFrameworkCore;

namespace SGT.Controllers
{
    public class StatusController : Controller
    {
        private readonly MVCDbContext mVCDbContext1;

        public StatusController(MVCDbContext mVCDbContext)
        {
            mVCDbContext1 = mVCDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var status = await mVCDbContext1.Status.ToListAsync();
            return View(status);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> Add(AddStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Status = new Status
                {
                    Id = Guid.NewGuid(),
                    Descricao = model.Descricao,
                };
                await mVCDbContext1.Status.AddAsync(Status);
                await mVCDbContext1.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var Status = await mVCDbContext1.Status.FirstOrDefaultAsync(x => x.Id == id);

            if (Status != null)
            {
                var viewModel = new UpdateStatusViewModel
                {
                    Id = Status.Id,
                    Descricao = Status.Descricao,
                };
                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateStatusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Status = await mVCDbContext1.Status.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (Status != null)
                {
                    Status.Descricao = model.Descricao;

                    await mVCDbContext1.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateStatusViewModel model)
        {
            var Status = await mVCDbContext1.Status.FindAsync(model.Id);

            if (Status != null)
            {
                mVCDbContext1.Status.Remove(Status);
                await mVCDbContext1.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}