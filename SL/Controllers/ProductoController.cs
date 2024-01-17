using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            List<object> list = BL.Producto.GetAll();

            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult Add(BL.Producto producto)
        {
            bool correct = BL.Producto.Add(producto);

            if (correct)
            {
                return Ok("Se ha agregado correctamente");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{idProducto}")]
        public IActionResult Delete(int idProducto)
        {
            bool correct = BL.Producto.Delete(idProducto);

            if (correct)
            {
                return Ok("Se ha eliminado correctamente");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{idProducto}")]
        public IActionResult GetById(int idProducto)
        {
            BL.Producto producto = BL.Producto.GetById(idProducto);

            if (producto != null)
            {
                return Ok(producto);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("{idProducto}")]
        public IActionResult Update(int idProducto, [FromBody] BL.Producto itemProducto)
        {
            itemProducto.IdProducto = idProducto;
            bool correct = BL.Producto.Update(itemProducto);

            if (correct)
            {
                return Ok("Se ha actualizado correctamente");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
