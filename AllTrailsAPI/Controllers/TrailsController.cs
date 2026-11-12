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
        public async Task<ActionResult<IEnumerable<Trail>>> GetAllTrails()
        {
            var sql = @"SELECT t.*, c.* 
                FROM Trail t 
                INNER JOIN Category c ON t.CategoryId = c.Id";

            var trails = await _db.QueryAsync<Trail, Category, Trail>( // table 1, table 2, return type
                sql,
                (trail, category) =>
                { 
                    // function executes for each row returned
                    trail.Category = category;
                    return trail;
                },
                splitOn: "Id" // specify what column to split on (the second table's Id column in this case)
                );

            return Ok(trails);
        }

    }
}
