namespace Hamburguesas.Models.ViewModels
{
    public class PromocionViewModel
    {
        public Hambu Seleccionada { get; set; }
        public string NameAnterior { get; set; } = null!;
        public string NameSiguiente { get; set; }= null!;
    }
    public class Hambu
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public decimal PrecioDescuento { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}
