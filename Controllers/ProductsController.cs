using Microsoft.AspNetCore.Mvc;
using CalorieTracker.Context;
using CalorieTracker.Models;

namespace CalorieTracker.Controllers
{
    [Route("api/products")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult List()
        {
            try
            {
                using (var context = new CalorieContext())
                {
                    var products = context.Products.ToList();
                    return Ok(products);
                }
            }
            catch (Exception exp)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public ActionResult Add([FromBody] Product product)
        {
            try
            {
                using (var context = new CalorieContext())
                {
                    context.Products.Add(product);
                    context.SaveChanges();
                    return StatusCode(201);
                }
            }
            catch (Exception exp)
            {
                return StatusCode(500, "Ошибка сервера");
            }
        }
    }
}
