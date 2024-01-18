using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
            BL.Producto producto = GetItem(idProducto);

            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (producto != null)
                {
                    producto.Cantidad = 1;
                    ventaModel.Carrito.Add(producto);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(ventaModel.Carrito));
                    ViewBag.Mensaje = "Se ha agregado correctamente";
                }
            }
            else
            {
                GetCart(ventaModel);

                foreach (BL.Producto itemProducto in ventaModel.Carrito)
                {
                    if (producto.IdProducto == itemProducto.IdProducto)
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
                    producto.Cantidad = 1;
                    ventaModel.Carrito.Add(producto);
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

        private static readonly string baseURL = "http://localhost:5295/api/Producto";
        public static BL.Producto GetItem(int idProducto)
        {
            BL.Producto producto = new BL.Producto();

            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync($"{baseURL}/{idProducto}");
                responseTask.Wait();

                var resultService = responseTask.Result;

                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result);
                    readTask.Wait();

                    producto.IdProducto = resultJSON["idProducto"];
                    producto.Nombre = resultJSON["nombre"];
                    producto.CodigoBarras = resultJSON["codigoBarras"];
                    producto.Descripcion = resultJSON["descripcion"];
                    producto.Precio = resultJSON["precio"];
                    producto.Categoria = new BL.Categoria();
                    producto.Categoria.IdCategoria = resultJSON["categoria"]["idCategoria"];
                    producto.Categoria.Nombre = resultJSON["categoria"]["nombre"];
                    producto.PrecioVenta = resultJSON["precioVenta"];
                    producto.Descuento = resultJSON["descuento"];
                }
            }

            return producto;
        }
    }
}