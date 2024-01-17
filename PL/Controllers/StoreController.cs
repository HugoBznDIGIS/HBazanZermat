using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult GetAll()
        {
            BL.Producto producto = new BL.Producto();
            producto.Productos = ProductoController.GetAllProducto();

            return View(producto);
        }

        public IActionResult AddCart(int idProducto) 
        {
            bool existe = false;
            VentaModel ventaModel = new VentaModel();
            ventaModel.Carrito = new List<object>();
            BL.Producto producto = BL.Producto.GetById(idProducto);

            if (HttpContext.Session.GetString("Carrito") == null) 
            {
                if (producto != null)
                {
                    BL.Producto productoItem = producto;
                    productoItem.Cantidad = 1;
                    ventaModel.Carrito.Add(productoItem);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(ventaModel.Carrito));
                    ViewBag.Mensaje = "Se ha agregado correctamente";
                }
            }
            else
            {
                BL.Producto productoItem = producto;
                GetCart(ventaModel);

                foreach (BL.Producto itemProducto in ventaModel.Carrito)
                {
                    if (productoItem.IdProducto == itemProducto.IdProducto)
                    {
                        itemProducto.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe == true)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(ventaModel.Carrito));
                    ViewBag.Mensaje = "Se ha agregado correctamente";
                }
                else
                {
                    productoItem.Cantidad = 1;
                    ventaModel.Carrito.Add(productoItem);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(ventaModel.Carrito));
                    ViewBag.Mensaje = "Se ha agregado correctamente un nuevo elemento";
                }
            }

            return PartialView("Modal");
        }

        // Se obtienen los items del carrito
        public VentaModel GetCart(VentaModel ventaModel)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var obj in ventaSession)
            {
                BL.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<BL.Producto>(obj.ToString());
                ventaModel.Carrito.Add(objProducto);
            }

            return ventaModel;
        }

        public IActionResult Cart()
        {
            VentaModel carrito = new VentaModel();
            carrito.Carrito = new List<object>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCart(carrito);
                return View(carrito);
            }
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Carrito");
            return RedirectToAction("Cart");
        }
    }
}
