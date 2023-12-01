using Hamburguesas.Models.Entities;
using Hamburguesas.Models.ViewModels;
using Hamburguesas.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hamburguesas.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ClasificacionRepository repository, MenuRepository menuRepository)
        {
            context = repository;
            MenuRepository = menuRepository;
        }

        public ClasificacionRepository context { get; }
        public MenuRepository MenuRepository { get; }

        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("~/menu")]
        [Route("~/menu/{id}")]
        public IActionResult Menu(string id)
        {
            MenuViewModel datos = new MenuViewModel();
            datos.ListaClasifs = context.GetAll()
                .Select(x=> new ClasificacionModel()
                {
                    Nombre=x.Nombre,
                    Hamburguesas=x.Menu.Select(h=>
                    new Hamburguesa()
                    {
                        Id=h.Id,
                        Nombre=h.Nombre,
                        Precio=(decimal)h.Precio
                    })
                });
                
            if (id==null)
            {
                int HambuId = datos.ListaClasifs.FirstOrDefault().Hamburguesas.FirstOrDefault().Id;
                datos.HamburguesaSeleccionada = MenuRepository.GetAll().Where(x=>x.Id==HambuId)
                    .Select(x => new HamburguesaSeleccionada()
                    {
                        Id = x.Id,
                        Descripcion = x.Descripción
                    })
                    .FirstOrDefault()
                    ?? new HamburguesaSeleccionada()
                    ;
            }
            else
            {
                datos.HamburguesaSeleccionada = MenuRepository.GetAll()
                    .Where(x=> x.Nombre==id.Replace("-"," "))
                    .Select(x => new HamburguesaSeleccionada()
                    {
                        Id = x.Id,
                        Descripcion = x.Descripción
                    })
                    .FirstOrDefault()
                    ?? new HamburguesaSeleccionada()
                    ;
            }


            return View(datos); 
        }
        [Route("~/promociones")]
        [Route("~/promociones/{id}")]
        public IActionResult Promociones(string id)
        {
            PromocionViewModel vm = new PromocionViewModel();
            var array = MenuRepository.GetAll().Where(x => x.PrecioPromocion > 0).Select(x => x.Id).ToArray();
            Menu proye = new();
            if (array.Length==0)
            {
                return RedirectToAction("Menu");
            }

            if (id!=null && id!="")
            {
                id = id.Replace("-", " ");
                proye = MenuRepository.GetAll().First(x=>x.Nombre==id);
                if (proye == null) return RedirectToAction("Menu");
                int indice = Array.IndexOf(array, proye.Id);
                var HambuSiguiente = MenuRepository.GetAll().FirstOrDefault(x => x.Id == ((indice + 1) < array.Length ? array[indice + 1] : array[0]));
                vm.NameSiguiente = HambuSiguiente != null ? HambuSiguiente.Nombre : id;
                var HambuAnterior = MenuRepository.GetAll().FirstOrDefault(x => x.Id == ((indice - 1) != -1 ? array[indice - 1] : array[array.Length-1]));
                vm.NameAnterior = HambuAnterior != null ? HambuAnterior.Nombre : "";
            }
            else if (id==null || id=="")
            {
                proye = MenuRepository.GetAll().Where(x=>x.PrecioPromocion>0).First();
                
                int indice = Array.IndexOf(array, proye.Id);
                
                vm.Seleccionada = new Hambu()
                {
                    Id = proye.Id,
                    Nombre = proye.Nombre,
                    Precio = (decimal)proye.Precio,
                    PrecioDescuento = (decimal)(proye.PrecioPromocion != null ? proye.PrecioPromocion : 0),
                    Descripcion = proye.Descripción
                };
                var HambuSiguiente = MenuRepository.GetAll().FirstOrDefault(x => x.Id == ((indice + 1) < array.Length ? array[indice + 1] : array[0]));
                vm.NameSiguiente = HambuSiguiente!=null?HambuSiguiente.Nombre:"";
                vm.NameAnterior = "#";
            }
            vm.Seleccionada = new Hambu()
            {
                Id = proye.Id,
                Nombre = proye.Nombre,
                Precio = (decimal)proye.Precio,
                PrecioDescuento = (decimal)(proye.PrecioPromocion!=null?proye.PrecioPromocion:0),
                Descripcion = proye.Descripción
            };
            return View(vm);
        }
    }
}
