using AllTrailsAPI.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AllTrailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : ControllerBase
    {
        private readonly IDbConnection _db;

        // Constructor
        public TrailsController(IDbConnection db)
        {
            _db = db;
        }

        // GET api/trails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trail>>> GetAll()
        {
            var sql = "SELECT * FROM Trail";

            var trails = await _db.QueryAsync<Trail>(sql);

            return Ok(trails);
        }

    }
}
