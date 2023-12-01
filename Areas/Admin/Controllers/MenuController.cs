using Hamburguesas.Areas.Admin.Models.ViewModels;
using Hamburguesas.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hamburguesas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        public MenuController(ClasificacionRepository repository, MenuRepository menuRepository)
        {
            ClasificacionRepository = repository;
            MenuRepository = menuRepository;
        }

        public ClasificacionRepository ClasificacionRepository { get; }
        public MenuRepository MenuRepository { get; }

        public IActionResult Index()
        {
            return View();
        }
        [Route("~/admin/menu/menu")]
        public IActionResult Menu()
        {
            var datos = ClasificacionRepository
                .GetAll()
                .Select(x => new AdminMenuViewModel()
                {
                    NombreClasificacion = x.Nombre,
                    Hamburguesas = x.Menu.Select(y => new HamburguesaModel()
                    {
                        Id = y.Id,
                        Name = y.Nombre,
                        Descripcion = y.Descripción,
                        Precio = (decimal)y.Precio,
                        PrecioDescuento = (decimal)(y.PrecioPromocion == null ? 0 : y.PrecioPromocion)
                    })
                });
            return View(datos);
        }
        [HttpGet]
        [Route("~/admin/menu/agregar")]
        public IActionResult Agregar()
        {
            AdminAgregarViewModel vm = new AdminAgregarViewModel();
            vm.Clasificaciones = ClasificacionRepository
                .GetAll()
                .Select(x => new ClasificacionModel()
                {
                    Id = x.Id,
                    Name = x.Nombre
                });
            return View(vm);
        }
        [HttpPost]
        [Route("~/admin/menu/agregar")]
        public IActionResult Agregar(AdminAgregarViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Menu.Nombre))
            {
                ModelState.AddModelError("", "Agregue nombre a la hamburguesa");
            }
            if (string.IsNullOrWhiteSpace(vm.Menu.Descripción))
            {
                ModelState.AddModelError("", "Agregue descripción");
            }
            if (vm.Menu.Precio == 0)
            {
                ModelState.AddModelError("", "Agregue un precio");
            }
            if (vm.Menu.IdClasificacion == 0)
            {
                ModelState.AddModelError("", "Selecciona una clasificacion");
            }

            if (vm.File != null)
            {
                if (vm.File.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Debe ser imagen png");
                }
                if (vm.File.Length > (500 * 1024))
                {
                    ModelState.AddModelError("", "Solo archivos menores a 500kb");
                }
            }
            //ModelState.Clear();
            if (ModelState.IsValid)
            {
                MenuRepository.Insert(vm.Menu);
                if (vm.File == null)
                {
                    System.IO.File.Copy("wwwroot/images/burger.png", $"wwwroot/hamburguesas/{vm.Menu.Id}.png");
                }
                else
                {
                    System.IO.FileStream fs =
                    System.IO.File.Create($"wwwroot/hamburguesas/{vm.Menu.Id}.png");
                    vm.File.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Menu");
            }
            vm.Clasificaciones = ClasificacionRepository
                .GetAll()
                .Select(x => new ClasificacionModel()
                {
                    Id = x.Id,
                    Name = x.Nombre
                });
            return View(vm);

        }
        [HttpGet]
        [Route("~/admin/promocion/{id}")]
        public IActionResult Promocion(string id)
        {
            id = id.Replace("-", " ");
            var menu = MenuRepository.GetMenuByName(id);
            PromocionViewModel vm = new PromocionViewModel()
            {
                Nombre = menu != null ? menu.Nombre : "",
                Precio = (decimal)(menu != null ? menu.Precio : 0),
                PrecioPromo = (decimal)(menu.PrecioPromocion == null ? 0 : menu.PrecioPromocion),
            };

            return View(vm);
        }
        [HttpPost]
        [Route("~/admin/menu/promocion")]
        public IActionResult Promocion(PromocionViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Nombre))
            {
                ModelState.AddModelError("", "No quite el nombre");
            }
            if (vm.Precio == 0)
            {
                ModelState.AddModelError("", "No quite el precio");
            }
            if (vm.PrecioPromo == 0)
            {
                ModelState.AddModelError("", "Agregue un precio de promocion");
            }
            if (vm.PrecioPromo >= vm.Precio)
            {
                ModelState.AddModelError("", "No puede ser menor o igual");
            }
            var menu = MenuRepository.GetMenuByName(vm.Nombre);
            if (vm.Precio != (decimal)menu.Precio)
            {
                ModelState.AddModelError("", "No cambie el precio original");
            }
            if (menu != null && ModelState.IsValid)
            {
                menu.PrecioPromocion = (double)vm.PrecioPromo;
                MenuRepository.Update(menu);
                return RedirectToAction("Menu");
            }
            return View(vm);
        }

        [HttpGet]
        [Route("~/admin/editar/{id}")]
        public IActionResult Editar(string id)
        {
            id = id.Replace("-", " ");
            EditarMenuViewModel vm = new EditarMenuViewModel();
            vm.Hamburguesa = MenuRepository.GetAll()
                .First(x => x.Nombre == id);
            vm.Clasificaciones = ClasificacionRepository
                .GetAll()
                .Select(x => new ClasificacionModel()
                {
                    Id = x.Id,
                    Name = x.Nombre
                });

            return View(vm);
        }

        [HttpPost]
        [Route("~/admin/editar")]
        public IActionResult Editar(EditarMenuViewModel vm) 
        {
            if (string.IsNullOrWhiteSpace(vm.Hamburguesa.Nombre))
            {
                ModelState.AddModelError("", "Agregue nombre a la hamburguesa");
            }
            if (string.IsNullOrWhiteSpace(vm.Hamburguesa.Descripción))
            {
                ModelState.AddModelError("", "Agregue descripción");
            }
            if (vm.Hamburguesa.Precio == 0)
            {
                ModelState.AddModelError("", "Agregue un precio");
            }
            if (vm.Hamburguesa.IdClasificacion == 0)
            {
                ModelState.AddModelError("", "Selecciona una clasificacion");
            }

            if (vm.File != null)
            {
                if (vm.File.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Debe ser imagen png");
                }
                if (vm.File.Length > (500 * 1024))
                {
                    ModelState.AddModelError("", "Solo archivos menores a 500kb");
                }
            }
            //ModelState.Clear();
            if (ModelState.IsValid)
            {
                MenuRepository.Update(vm.Hamburguesa);
                if (vm.File!= null)
                {
                    System.IO.FileStream fs =
                    System.IO.File.Create($"wwwroot/hamburguesas/{vm.Hamburguesa.Id}.png");
                    vm.File.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Menu");
            }
            vm.Clasificaciones = ClasificacionRepository
                .GetAll()
                .Select(x => new ClasificacionModel()
                {
                    Id = x.Id,
                    Name = x.Nombre
                });
            return View(vm);
        }
        [Route("~/admin/eliminar/{id}")]
        public IActionResult Eliminar(string id)
        {
            id = id.Replace("-", " ");
            EliminarMenuViewModel vm = new EliminarMenuViewModel();
            var dato = MenuRepository.GetAll().Where(x=>x.Nombre==id).First();
            if (dato!=null)
            {
                vm.Nombre = dato.Nombre;
            }
            else
            {
                return RedirectToAction("Menu");
            }
            return View(vm);
        }
        [HttpPost]
        [Route("~/admin/eliminar")]
        public IActionResult Eliminar(EliminarMenuViewModel vm)
        {
            if (string.IsNullOrWhiteSpace( vm.Nombre))
            {
                ModelState.AddModelError("", "No quites el nombre");
            }
            var dato = MenuRepository.GetAll().Where(x => x.Nombre == vm.Nombre).First();
            if (ModelState.IsValid)
            {
                if (dato != null)
                {
                    MenuRepository.Delete(dato);
                    string ruta = $"wwwroot/hamburguesas/{dato.Id}.png";
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    return RedirectToAction("Menu");
                }
            }
            return View(vm);
        }
        [Route("~/admin/quitarpromocion/{id}")]
        public IActionResult QuitarPromo(string id)
        {
            id = id.Replace("-", " ");
            var proye = MenuRepository.GetAll().Where(x=>x.Nombre==id).First();
            PromocionViewModel vm = new PromocionViewModel()
            {
                Nombre= proye.Nombre,
                Precio=(decimal)proye.Precio,
                PrecioPromo=(decimal)proye.PrecioPromocion
            };
            return View(vm);
        }
        [HttpPost]
        [Route("~/admin/quitarpromocion")]
        public IActionResult QuitarPromo(PromocionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var proye = MenuRepository.GetAll().Where(x=>x.Nombre==vm.Nombre).First();
                proye.PrecioPromocion = null;
                MenuRepository.Update(proye);
                return RedirectToAction("Menu");
            }
            else
            {
                return View(vm);
            }
        }
    }
}
