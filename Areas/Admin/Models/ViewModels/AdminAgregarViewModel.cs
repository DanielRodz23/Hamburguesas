using Hamburguesas.Models.Entities;

namespace Hamburguesas.Areas.Admin.Models.ViewModels
{
    public class AdminAgregarViewModel
    {
        public Menu Menu { get; set; } = new Menu();
        public IEnumerable<ClasificacionModel>? Clasificaciones { get; set; } 
        public IFormFile? File { get; set; }
    }
    public class ClasificacionModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
