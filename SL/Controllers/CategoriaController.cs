using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            List<object> list = BL.Categoria.GetAll();

            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
