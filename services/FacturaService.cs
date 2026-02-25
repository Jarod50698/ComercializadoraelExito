using ComercializadoraelExito.Models;

namespace ComercializadoraelExito.Services
{
    public class FacturaService
    {
        public void CalcularTotales(Factura factura)
        {
            factura.Subtotal = factura.Detalles
                .Sum(d => d.Cantidad * d.PrecioUnitario);

            factura.Impuesto = factura.Subtotal * 0.13m;
            factura.Total = factura.Subtotal + factura.Impuesto;
        }
    }
}