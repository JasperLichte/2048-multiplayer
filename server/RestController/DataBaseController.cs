using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using server.model;
using server.model.interfaces;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {      
        readonly IDBQueryHelper dbquery;

        public DataBaseController()
        {
            dbquery=new MySQLHandler();
        }

       

        [HttpGet("allGames/{numberOfPlayer}", Name="GetGameData")]
        public ActionResult<List<RestGame>> GetAll(int numberOfPlayer)
        {
            return  dbquery.loadGameData(numberOfPlayer);
        }
        
        [HttpGet("getHighscore", Name = "GetHighcore")]
        public ActionResult<List<RestScore>> GetByName()
        {
            return dbquery.highestScore();// NotFound();
        }

        [HttpGet("countGameOfPlayer", Name = "countGameOfPlayer")]
        public ActionResult<List<RestPlayerCountGame>> countGameOfPlayer()
        {
            return dbquery.playerCountGame();
        }

    }    
    
}
