using System;
using System.Collections.Generic;

namespace DL;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public int CodigoBarras { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int? IdCategoria { get; set; }

    public decimal PrecioVenta { get; set; }

    public decimal? Descuento { get; set; }

    public virtual Categorium? IdCategoriaNavigation { get; set; }
}
