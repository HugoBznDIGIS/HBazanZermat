namespace UT
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Categoria()
        {
            List<object> list = BL.Categoria.GetAll();
        }

        [TestMethod]
        public void Producto() 
        {
            List<object> list = BL.Producto.GetAll();
        }
    }
}