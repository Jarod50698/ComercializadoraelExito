using System.ComponentModel.DataAnnotations;

namespace ComercializadoraelExito.Models
{
    public class Factura
    {
        public int Id { get; set; }

        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Subtotal { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
    }
}