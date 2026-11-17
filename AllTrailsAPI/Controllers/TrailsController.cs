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

        // GET api/trails/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Trail>>> GetTrailById(int id)
        {
            var sql = @"SELECT t.*, c.*, r.*, u.*
                FROM Trail t
                INNER JOIN Category c ON t.CategoryId = c.Id
                LEFT OUTER JOIN HikerReview r ON  t.Id = r.TrailId
                INNER JOIN UserSnapshot u ON u.UserId = r.CreatedBy
                WHERE t.Id = @Id";

            // Create a null Trail object that we populate from the query results
            Trail? trail = null;

            await _db.QueryAsync<Trail, Category, Review, User, Trail>( // table 1, table 2, table 3, table 4, return type
                sql,
                (t, c, r, u) =>
                {
                    // populate the trail object with the first row of data
                    if(trail == null)
                    {
                        trail = t;
                        trail.Category = c;
                        trail.Reviews = new List<Review>();
                    }

                    // add the review to the trail's Reviews list if it exists
                    if(r != null)
                    {
                        r.User = u;
                        trail.Reviews.Add(r);
                    }

                    return trail;
                },
                new {Id = id}, // replace @Id in the sql
                splitOn: "Id,Id,UserId" // specify what column to split on
                );

            return Ok(trail);
        }
    }
}
