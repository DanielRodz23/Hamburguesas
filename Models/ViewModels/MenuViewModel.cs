namespace Hamburguesas.Models.ViewModels
{
    public class MenuViewModel
    {
        public IEnumerable<ClasificacionModel> ListaClasifs { get; set; } = null!;
        public HamburguesaSeleccionada HamburguesaSeleccionada { get; set; } = null!;
    }
    public class ClasificacionModel
    {
        public string Nombre { get; set; } = null!;
        public IEnumerable<Hamburguesa> Hamburguesas { get; set; } = null!;
    }
    public class Hamburguesa
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
    }
    public class HamburguesaSeleccionada
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
    }

}
