using DemoWebAPI.Entities;
using DemoWebAPI.Hubs;
using DemoWebAPI.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MyHub _hub;

        public ArticleController(DataContext context, MyHub myHub)
        {
            _context = context;
            _hub = myHub;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_context.Articles);
            }
            catch (Exception EX)
            {
                return NotFound(EX.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Article a)
        {
            _context.Articles.Add(a);
            if (_context.SaveChanges() > 0)
            {
                _hub.NotifyArticleUpdate();
            }


            return Ok();
        }

        [HttpGet]
        [Route("getById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_context.Articles.FirstOrDefault(x => x.Id == id));
        }

    }
}
