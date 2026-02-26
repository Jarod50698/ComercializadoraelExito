using Microsoft.AspNetCore.Mvc;
using ComercializadoraelExito.Data;
using ComercializadoraelExito.Models;

namespace ComercializadoraelExito.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // READ
        public IActionResult Index()
        {
            return View(_context.Productos.ToList());
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            if (!ModelState.IsValid)
                return View(producto);

            _context.Productos.Add(producto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Producto producto)
        {
            if (id != producto.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(producto);

            _context.Update(producto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Find(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}