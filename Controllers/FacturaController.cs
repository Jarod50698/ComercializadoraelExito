using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComercializadoraelExito.Models;
using ComercializadoraelExito.Services;
using System.Text;
using ComercializadoraelExito.Data;

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
            ViewBag.Productos = _context.Productos.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(string nombreCliente, int productoId, int cantidad)
        {
            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                ModelState.AddModelError("", "El nombre del cliente es obligatorio.");
            }

            if (cantidad <= 0)
            {
                ModelState.AddModelError("", "La cantidad debe ser mayor a cero.");
            }

            var producto = _context.Productos.Find(productoId);

            if (producto == null)
            {
                ModelState.AddModelError("", "El producto seleccionado no existe.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _context.Productos.ToList();
                return View();
            }

            var factura = new Factura
            {
                NombreCliente = nombreCliente.Trim(),
                Fecha = DateTime.Now
            };

            factura.Detalles.Add(new DetalleFactura
            {
                ProductoId = producto!.Id,
                NombreProducto = producto.Nombre,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio
            });

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