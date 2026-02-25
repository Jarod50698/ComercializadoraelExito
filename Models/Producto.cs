using System.ComponentModel.DataAnnotations;

namespace ComercializadoraelExito.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public decimal Precio { get; set; }
    }
}