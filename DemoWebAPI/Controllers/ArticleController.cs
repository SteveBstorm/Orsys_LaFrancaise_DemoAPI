using DemoWebAPI.Entities;
using DemoWebAPI.Hubs;
using DemoWebAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    //[Authorize("AdminPolicy")]
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
        [AllowAnonymous]
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
        [Authorize("AdminPolicy")]
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

        [Authorize("IsConnected")]
        [HttpGet]
        [Route("getById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_context.Articles.FirstOrDefault(x => x.Id == id));
        }

    }
}
