using ecycle_be.Models;
using ecycle_be.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecycle_be.Controllers
{
    [ApiController]
    [Route("article")]
    public class ArticleController(ArticleService articleService) : ControllerBase
    {
        private ArticleService _articleService = articleService;

        [HttpGet("")]
        public async Task<IActionResult> GetArticles()
        {
            List<Artikel> articles = await _articleService.GetArticles();
            return Ok(articles);
        }
    }
}
