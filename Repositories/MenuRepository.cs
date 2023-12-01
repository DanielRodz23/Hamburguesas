using Hamburguesas.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hamburguesas.Repositories
{
    public class MenuRepository : Repository<Menu>
    {
        public MenuRepository(NeatContext context) : base(context){}

        public IEnumerable<Menu> GetAll()
        {
            return Context.Menu
                .Include(x => x.IdClasificacionNavigation);
        }
        public Menu? GetMenuByName(string name)
        {
            return Context.Menu.Include(x => x.IdClasificacionNavigation).Where(x => x.Nombre == name).FirstOrDefault();
        }
         
    }
}
