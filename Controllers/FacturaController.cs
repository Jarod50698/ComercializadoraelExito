using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComercializadoraelExito.Models;
using ComercializadoraelExito.Services;
using System.Text;
using ComercializadoraelExito.Data;
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
    if (!ModelState.IsValid || vm.Detalles == null || !vm.Detalles.Any(d => d.Cantidad > 0))
    {
        vm.ProductosDisponibles = _context.Productos.ToList();
        ModelState.AddModelError("", "Debe seleccionar al menos un producto con cantidad mayor a cero.");
        return View(vm);
    }

    var factura = new Factura
    {
        NombreCliente = vm.NombreCliente.Trim(),
        Fecha = DateTime.Now,
        Detalles = vm.Detalles.Where(d => d.Cantidad > 0).ToList()
    };

    _service.CalcularTotales(factura);

    _context.Facturas.Add(factura);
    _context.SaveChanges();

    return RedirectToAction("Historial");
}

        public IActionResult Descargar(int id)
        {
            var factura = _context.Facturas
                .Include(f => f.Detalles)
                .FirstOrDefault(f => f.Id == id);

            if (factura == null)
            {
                return NotFound("La factura solicitada no existe.");
            }

            var sb = new StringBuilder();

            sb.AppendLine("FACTURA");
            sb.AppendLine("Cliente: " + factura.NombreCliente);
            sb.AppendLine("Fecha: " + factura.Fecha);
            sb.AppendLine("--------------------------------");

            foreach (var d in factura.Detalles)
            {
                sb.AppendLine($"{d.NombreProducto} x{d.Cantidad} - ₡{d.PrecioUnitario}");
            }

            sb.AppendLine("--------------------------------");
            sb.AppendLine("Subtotal: ₡" + factura.Subtotal);
            sb.AppendLine("Impuesto (13%): ₡" + factura.Impuesto);
            sb.AppendLine("Total: ₡" + factura.Total);

            return File(
                Encoding.UTF8.GetBytes(sb.ToString()),
                "text/plain",
                $"Factura_{factura.Id}.txt"
            );
        }
    }
}