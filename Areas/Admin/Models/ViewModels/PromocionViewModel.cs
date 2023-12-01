namespace Hamburguesas.Areas.Admin.Models.ViewModels
{
    public class PromocionViewModel
    {
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public decimal PrecioPromo { get; set; }
    }
}
