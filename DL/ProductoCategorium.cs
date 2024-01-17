using System;
using System.Collections.Generic;

namespace DL;

public partial class ProductoCategorium
{
    public int? IdProducto { get; set; }

    public int? IdCategoria { get; set; }

    public virtual Categorium? IdCategoriaNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
