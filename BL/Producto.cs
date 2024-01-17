using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre es de maximo 100 caracteres y minimo de 1")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El codigo es requerido")]
        public int CodigoBarras { get; set; }

        [Required(ErrorMessage = "La descripcion es requerida")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "La descripcion es de maximo 500 caracteres y minimo de 1")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [RegularExpression(@"^\d{1,10}(\.\d{1,10})?$", ErrorMessage = "El precio debe tener hasta 10 dígitos enteros y hasta 10 dígitos decimales.")]
        public decimal Precio { get; set; }

        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "El precio venta es requerido")]
        [RegularExpression(@"^\d{1,10}(\.\d{1,10})?$", ErrorMessage = "El precio debe tener hasta 20 dígitos enteros y hasta 10 dígitos decimales.")]
        public decimal PrecioVenta { get; set; }

        [RegularExpression(@"^\d{1,10}(\.\d{1,10})?$", ErrorMessage = "El descuento debe ser en porcentaje")]
        public decimal? Descuento { get; set; }
        public List<object>? Productos { get; set; }
        public int? Cantidad { get; set; }

        public static List<object> GetAll()
        {
            List<object> list = new List<object>();

            try
            {
                using (DL.HbazanZermatContext context = new DL.HbazanZermatContext())
                {
                    var query = (from objProducto in context.Productos
                                 join objCategoria in context.Categoria on objProducto.IdCategoria equals objCategoria.IdCategoria
                                 select new
                                 {
                                     IdProducto = objProducto.IdProducto,
                                     NombreProducto = objProducto.Nombre,
                                     CodigoBarras = objProducto.CodigoBarras,
                                     Descripcion = objProducto.Descripcion,
                                     Precio = objProducto.Precio,
                                     PrecioVenta = objProducto.PrecioVenta,
                                     Descuento = objProducto.Descuento,
                                     IdCategoria = objCategoria.IdCategoria,
                                     NombreCategoria = objCategoria.Nombre
                                 });

                    if (query != null)
                    {
                        foreach (var itemProducto in query)
                        {
                            Producto productoItemList = new Producto();

                            productoItemList.IdProducto = itemProducto.IdProducto;
                            productoItemList.Nombre = itemProducto.NombreProducto;
                            productoItemList.CodigoBarras = itemProducto.CodigoBarras;
                            productoItemList.Descripcion = itemProducto.Descripcion;
                            productoItemList.Precio = itemProducto.Precio;
                            productoItemList.PrecioVenta = itemProducto.PrecioVenta;
                            productoItemList.Descuento = itemProducto.Descuento;
                            productoItemList.Categoria = new Categoria();
                            productoItemList.Categoria.IdCategoria = (int)itemProducto.IdCategoria;
                            productoItemList.Categoria.Nombre = itemProducto.NombreCategoria;

                            list.Add(productoItemList);
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

        public static bool Delete(int idProducto)
        {
            bool correct;

            try
            {
                using (DL.HbazanZermatContext context = new DL.HbazanZermatContext())
                {
                    var affectedRow = context.Database.ExecuteSqlRaw($"DeleteProducto {idProducto}");

                    if (affectedRow > 0)
                    {
                        correct = true;
                    }
                    else
                    {
                        correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

            return correct;
        }

        public static bool Add(Producto itemProducto)
        {
            bool correct;

            try
            {
                using (DL.HbazanZermatContext context = new DL.HbazanZermatContext())
                {
                    var affectedRow = context.Database.ExecuteSqlRaw($"AddProducto '{itemProducto.Nombre}', {itemProducto.CodigoBarras}, '{itemProducto.Descripcion}', {itemProducto.Precio}, {itemProducto.Categoria.IdCategoria}, {itemProducto.PrecioVenta}, {itemProducto.Descuento}");

                    if (affectedRow > 0)
                    {
                        correct = true;
                    }
                    else
                    {
                        correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

            return correct;
        }

        public static Producto GetById(int idProducto)
        {
            Producto productoItem = new Producto();

            try
            {
                using (DL.HbazanZermatContext context = new DL.HbazanZermatContext())
                {
                    var queryById = (from objProducto in context.Productos
                                     join objCategoria in context.Categoria on objProducto.IdCategoria equals objCategoria.IdCategoria
                                     where objProducto.IdProducto == idProducto
                                     select new
                                     {
                                         IdProducto = objProducto.IdProducto,
                                         NombreProducto = objProducto.Nombre,
                                         CodigoBarras = objProducto.CodigoBarras,
                                         Descripcion = objProducto.Descripcion,
                                         Precio = objProducto.Precio,
                                         PrecioVenta = objProducto.PrecioVenta,
                                         Descuento = objProducto.Descuento,
                                         IdCategoria = objCategoria.IdCategoria,
                                         NombreCategoria = objCategoria.Nombre
                                     }).FirstOrDefault();

                    if (queryById != null)
                    {

                        productoItem.IdProducto = queryById.IdProducto;
                        productoItem.Nombre = queryById.NombreProducto;
                        productoItem.CodigoBarras = queryById.CodigoBarras;
                        productoItem.Descripcion = queryById.Descripcion;
                        productoItem.Precio = queryById.Precio;
                        productoItem.PrecioVenta = queryById.PrecioVenta;
                        productoItem.Descuento = queryById.Descuento;
                        productoItem.Categoria = new Categoria();
                        productoItem.Categoria.IdCategoria = queryById.IdCategoria;
                        productoItem.Categoria.Nombre = queryById.NombreCategoria;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

            return productoItem;
        }

        public static bool Update(Producto itemProducto)
        {
            bool correct;

            try
            {
                using (DL.HbazanZermatContext context = new DL.HbazanZermatContext())
                {
                    var affectedRow = context.Database.ExecuteSqlRaw($"UpdateProducto '{itemProducto.Nombre}', {itemProducto.CodigoBarras}, '{itemProducto.Descripcion}', {itemProducto.Precio}, {itemProducto.Categoria.IdCategoria}, {itemProducto.PrecioVenta}, { itemProducto.Descuento }, {itemProducto.IdProducto}");

                    if (affectedRow > 0)
                    {
                        correct = true;
                    }
                    else
                    {
                        correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

            return correct;
        }
    }
}