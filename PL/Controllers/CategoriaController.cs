using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PL.Controllers
{
    public class CategoriaController : Controller
    {
        private static readonly string baseURL = "http://localhost:5295/api/Categoria";

        public static List<object> GetAll()
        {
            List<object> list = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    var responseTask = client.GetAsync(baseURL);
                    responseTask.Wait();

                    var resultService = responseTask.Result;

                    if (resultService.IsSuccessStatusCode)
                    {
                        var readTask = resultService.Content.ReadAsStringAsync();
                        dynamic resultJSON = JArray.Parse(readTask.Result);

                        if (resultJSON != null)
                        {
                            foreach (var itemCategoria in resultJSON)
                            {
                                BL.Categoria categoria = new BL.Categoria();

                                categoria.IdCategoria = itemCategoria.idCategoria;
                                categoria.Nombre = itemCategoria.nombre;

                                list.Add(categoria);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

            return list;
        }
    }
}
