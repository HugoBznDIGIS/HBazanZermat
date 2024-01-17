namespace PL.Models
{
    public class VentaModel
    {
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public List<object> Carrito { get; set; }
    }
}
