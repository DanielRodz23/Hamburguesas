namespace Hamburguesas.Areas.Admin.Models.ViewModels
{
    public class AdminMenuViewModel
    {
        public string NombreClasificacion { get; set; } = null!;
        public IEnumerable<HamburguesaModel> Hamburguesas { get; set; }=null!;
    }
    public class HamburguesaModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public decimal PrecioDescuento { get; set; }
    }
}
