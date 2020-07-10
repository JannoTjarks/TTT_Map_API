using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using TTT_Map_API.Structs;

namespace TTT_Map_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Map_RatingsController : ControllerBase
    {
        // GET api/map_ratings
        [HttpGet]
        public ActionResult<IEnumerable<TableContent>> Get()
        {
            var database = Database.GetInstance();
            var tableContent = new List<TableContent>();            
            foreach (var mapName in database.GetAllMapNames())
            {
                tableContent.Add(new TableContent(mapName, database.GetMapAverageRating(database.GetMapIdByName(mapName))));
            }

            return tableContent;
        }

        // GET api/map_ratings/ttt_skyscraper
        //[HttpGet("{mapName}")]
        //public ActionResult<IEnumerable<TableContent>> Get(string mapName)
        //{
        //    var tableContent = new List<TableContent>();
        //    tableContent.Add(new TableContent(mapName, Database.GetMapAverageRating(Database.GetMapIdByName(mapName))));

        //    return tableContent;
        //}

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}        
    }
}
