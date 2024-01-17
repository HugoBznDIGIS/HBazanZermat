using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        private static readonly string baseURL = "http://localhost:5295/api/Producto";
        public IActionResult GetAll()
        {
            BL.Producto producto = new BL.Producto();
            producto.Productos = GetAllProducto();
            
            return View(producto);
        }

        public static List<object> GetAllProducto()
        {
            List<object> products = new List<object>(); 

            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync(baseURL);
                responseTask.Wait();

                var resultService = responseTask.Result;

                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsStringAsync();
                    dynamic resultJSON = JArray.Parse(readTask.Result);
                    readTask.Wait();

                    if (resultJSON != null)
                    {
                        foreach (var itemProducto in resultJSON)
                        {
                            BL.Producto productoItemList = new BL.Producto();
                            productoItemList.IdProducto = itemProducto.idProducto;
                            productoItemList.Nombre = itemProducto.nombre;
                            productoItemList.CodigoBarras = itemProducto.codigoBarras;
                            productoItemList.Descripcion = itemProducto.descripcion;
                            productoItemList.Precio = itemProducto.precio;
                            productoItemList.PrecioVenta = itemProducto.precioVenta;
                            productoItemList.Descuento = itemProducto.descuento;
                            productoItemList.Categoria = new BL.Categoria();
                            productoItemList.Categoria.IdCategoria = itemProducto.categoria.idCategoria;
                            productoItemList.Categoria.Nombre = itemProducto.categoria.nombre;

                            products.Add(productoItemList);
                        }
                    }
                }
            }

            return products;
        }

        public IActionResult Delete(int idProducto)
        {
            using (var client = new HttpClient())
            {
                var responseTask = client.DeleteAsync($"{baseURL}/{idProducto}");
                responseTask.Wait();

                var resultService = responseTask.Result;

                if (resultService.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se ha eliminado correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "Error al eliminar";
                }
            }

            return PartialView("Modal");
        }

        public IActionResult Form(int? idProducto)
        {
            BL.Producto producto = new BL.Producto();
            producto.Categoria = new BL.Categoria();
            producto.Categoria.Categorias = CategoriaController.GetAll();

            if (idProducto != null)
            {
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
                        producto.Categoria.IdCategoria = resultJSON["categoria"]["idCategoria"];
                        producto.Categoria.Nombre = resultJSON["categoria"]["nombre"];
                        producto.PrecioVenta = resultJSON["precioVenta"];
                        producto.Descuento = resultJSON["descuento"];
                    }
                }
            }
            return View(producto);
        }

        [HttpPost]
        public IActionResult Form(BL.Producto producto)
        {
            if (ModelState.IsValid)
            {
                if (producto.IdProducto == 0)
                {
                    using (var client = new HttpClient())
                    {
                        var responseTask = client.PostAsJsonAsync(baseURL, producto);
                        responseTask.Wait();

                        var resultService = responseTask.Result;

                        if (resultService.IsSuccessStatusCode)
                        {
                            ViewBag.Mensaje = "Se ha agregado correctamente";
                        }
                        else
                        {
                            ViewBag.Mensaje = "Ha ocurrido un error al agregar";
                        }
                    }
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var responseTask = client.PutAsJsonAsync($"{baseURL}/{producto.IdProducto}", producto);
                        responseTask.Wait();

                        var resultService = responseTask.Result;

                        if (resultService.IsSuccessStatusCode)
                        {
                            ViewBag.Mensaje = "Se ha actualizado correctamente";
                        }
                        else
                        {
                            ViewBag.Mensaje = "Ha ocurrido un error al actualizar";
                        }
                    }
                }

                return PartialView("Modal");
            }
            else
            {
                producto.Categoria.Categorias = CategoriaController.GetAll();
                return View(producto);
            }
        }
    }
}
