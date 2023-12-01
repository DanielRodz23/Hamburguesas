using Hamburguesas.Models.Entities;

namespace Hamburguesas.Areas.Admin.Models.ViewModels
{
    public class EditarMenuViewModel
    {
        public Menu Hamburguesa { get; set; }=null!;
        public IEnumerable<ClasificacionModel>? Clasificaciones { get; set; }
        public IFormFile? File { get; set; } 
    }
    public class HambuModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdClasificacion { get; set; }
    }
}
