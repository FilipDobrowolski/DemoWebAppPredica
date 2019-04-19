using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAppPredica.Models.Models.Journeys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAppPredica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {

        [HttpGet]
        public List<Journey> GetAllJourneys()
        {
            var journeys = new List<Journey>();
            return journeys;
        }

        [HttpGet("{userId}")]
        public List<Journey> GetUserJourneys(Guid userId)
        {
            var journeys = new List<Journey>();
            return journeys;
        }

        [HttpPost]
        public void CreateJourney([FromBody] Journey journey)
        {
            
        }

        [HttpPut("{journeyId}")]
        public void UpdateJourney(int journeyId, [FromBody] Journey journey)
        {
            
        }

        [HttpDelete("{journeyId}")]
        public void DeleteJourney(int journeyId)
        {
           
        }
    }
}