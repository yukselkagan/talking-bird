using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrendService.Data.Repository;
using TrendService.Models;

namespace TrendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrendsController : ControllerBase
    {
        private readonly ITrendRepository _trendRepository;
        public TrendsController(ITrendRepository trendRepository)
        {
            _trendRepository = trendRepository;
        }


        [HttpGet]
        public async Task<ActionResult> GetTrends()
        {
            var trends = await _trendRepository.Get(orderBy: q => q.OrderByDescending(x => x.PostCount));

            return Ok(trends);
        }


        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            var trends = await _trendRepository.Get();

            return Ok(trends);
        }




    }
}
