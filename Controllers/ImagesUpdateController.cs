using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServerMPImages.Data;
using WebServerMPImages.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServerMPImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesUpdateController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ImagesUpdateController(AppDbContext db)
        {
            _db = db;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public IEnumerable<ProductPhoto> Get(int id)
        {
            return _db.Products.Include(u => u.Photos.OrderBy(p => p.PhotoType)).FirstOrDefault(u => u.Id == id).Photos;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
