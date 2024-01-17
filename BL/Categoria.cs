using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Categoria
    {
        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Seleccione una categoria")]
        public int IdCategoria { get; set; }
        public string? Nombre { get; set; }
        public List<object>? Categorias { get; set; }

        public static List<object> GetAll()
        {
            List<object> list = new List<object>();

            try
            {
                using (DL.HbazanZermatContext context = new DL.HbazanZermatContext())
                {
                    var query = (from objCategoria in context.Categoria
                                 select new 
                                 { 
                                    IdCategoria = objCategoria.IdCategoria,
                                    Nombre = objCategoria.Nombre
                                 });

                    if (query != null)
                    {
                        foreach (var itemCategoria in query)
                        { 
                            Categoria categoriaItemList = new Categoria();
                            categoriaItemList.IdCategoria = itemCategoria.IdCategoria;
                            categoriaItemList.Nombre = itemCategoria.Nombre;

                            list.Add(categoriaItemList);
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
