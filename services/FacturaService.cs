using ComercializadoraelExito.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ComercializadoraelExito.Services
{
    public class FacturaService
    {
        public void CalcularTotales(Factura factura)
        {
            factura.Subtotal = factura.Detalles
                .Where(d => d.Cantidad > 0)
                .Sum(d => d.Cantidad * d.PrecioUnitario);

            factura.Impuesto = factura.Subtotal * 0.13m;
            factura.Total = factura.Subtotal + factura.Impuesto;
        }

        public byte[] GenerarPdf(Factura factura)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header().Text("COMERCIALIZADORA EL ÉXITO")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Factura #{factura.Id}").Bold();
                        col.Item().Text($"Cliente: {factura.NombreCliente}");
                        col.Item().Text($"Fecha: {factura.Fecha:dd/MM/yyyy HH:mm}");

                        col.Item().LineHorizontal(1);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Producto").Bold();
                                header.Cell().Text("Cant").Bold();
                                header.Cell().Text("Precio").Bold();
                                header.Cell().Text("Total").Bold();
                            });

                            foreach (var item in factura.Detalles.Where(x => x.Cantidad > 0))
                            {
                                table.Cell().Text(item.NombreProducto);
                                table.Cell().Text(item.Cantidad.ToString());
                                table.Cell().Text($"₡{item.PrecioUnitario:N2}");
                                table.Cell().Text($"₡{item.TotalLinea:N2}");
                            }
                        });

                        col.Item().LineHorizontal(1);

                        col.Item().AlignRight().Column(total =>
                        {
                            total.Item().Text($"Subtotal: ₡{factura.Subtotal:N2}");
                            total.Item().Text($"IVA (13%): ₡{factura.Impuesto:N2}");
                            total.Item().Text($"TOTAL: ₡{factura.Total:N2}")
                                .FontSize(14)
                                .Bold();
                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Gracias por su compra")
                        .FontSize(10);
                });
            }).GeneratePdf();
        }
    }
}