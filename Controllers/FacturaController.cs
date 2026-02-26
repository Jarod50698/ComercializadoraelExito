using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComercializadoraelExito.Data;
using ComercializadoraelExito.Models;
using ComercializadoraelExito.Services;
using ComercializadoraelExito.ViewModels;

namespace ComercializadoraelExito.Controllers
{
    public class FacturaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FacturaService _service;

        public FacturaController(AppDbContext context, FacturaService service)
        {
            _context = context;
            _service = service;
        }

        public IActionResult Historial()
        {
            var facturas = _context.Facturas
                .Include(f => f.Detalles)
                .ToList();

            return View(facturas);
        }

        public IActionResult Crear()
        {
            var vm = new FacturaViewModel
            {
                ProductosDisponibles = _context.Productos.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(FacturaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.ProductosDisponibles = _context.Productos.ToList();
                return View(vm);
            }

            var detallesValidos = vm.Detalles
                .Where(d => d.Cantidad > 0)
                .ToList();

            if (!detallesValidos.Any())
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto con cantidad mayor a cero.");
                vm.ProductosDisponibles = _context.Productos.ToList();
                return View(vm);
            }

            var factura = new Factura
            {
                NombreCliente = vm.NombreCliente,
                Fecha = DateTime.Now,
                Detalles = detallesValidos
            };

            _service.CalcularTotales(factura);

            _context.Facturas.Add(factura);
            _context.SaveChanges();

            return RedirectToAction("Historial");
        }

        public IActionResult DescargarPdf(int id)
        {
            var factura = _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefault(f => f.Id == id);

            if (factura == null)
                return NotFound();

            var pdf = _service.GenerarPdf(factura);

            return File(pdf, "application/pdf", $"Factura_{factura.Id}.pdf");
        }
    }
}