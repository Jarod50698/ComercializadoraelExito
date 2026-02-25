using ComercializadoraelExito.Models;

namespace ComercializadoraelExito.Models
{
    public class DetalleFactura
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int FacturaId { get; set; }

        public string NombreProducto { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal TotalLinea => Cantidad * PrecioUnitario;

        public Factura? Factura { get; set; }
    }
}