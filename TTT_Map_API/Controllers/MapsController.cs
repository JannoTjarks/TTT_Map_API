using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTT_Map_API.Structs;

namespace TTT_Map_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<TableContent>> Get()
        {
            var tableContent = new List<TableContent>();
            var database = Database.GetInstance();
            database.OpenConnection();
            foreach (var mapName in database.GetAllMapNames())
            {
                tableContent.Add(new TableContent(mapName, database.GetMapAverageRating(database.GetMapIdByName(mapName))));
            }

            database.CloseConnection();

            return tableContent;
        }

        [HttpPost]
        public StatusCodeResult Post([FromBody] Rating value)
        {
            var database = Database.GetInstance();
            database.OpenConnection();
            bool isPostSuccessful = database.InsertRating(value);
            database.CloseConnection();
            if (isPostSuccessful)
            {
                return new StatusCodeResult(200);
            }

            return new StatusCodeResult(400);
        }
    }
}
