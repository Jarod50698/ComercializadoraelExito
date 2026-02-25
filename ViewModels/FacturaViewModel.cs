using ComercializadoraelExito.Models;
using System.ComponentModel.DataAnnotations;

namespace ComercializadoraelExito.ViewModels
{
    public class FacturaViewModel
    {
        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        public List<Producto> ProductosDisponibles { get; set; } = new();

        public List<DetalleFactura> Detalles { get; set; } = new();

        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }
}