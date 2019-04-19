using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAppPredica.Models.Models.Journeys;
using DemoAppPredica.Processing.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAppPredica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        public readonly IJourneyService _journeyService;

        public JourneysController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpGet]
        public List<Journey> GetAllJourneys()
        {
            return _journeyService.GetAllJourneys();
        }

        [HttpGet("{userId}")]
        public List<Journey> GetUserJourneys(Guid userId)
        {
            return _journeyService.GetUserJourneys(userId);
        }

        [HttpPost]
        public void CreateJourney([FromBody] Journey journey)
        {
            _journeyService.CreateJourney(journey);
        }

        [HttpPut("{journeyId}")]
        public void UpdateJourney(int journeyId, [FromBody] Journey journey)
        {
            _journeyService.UpdateJourney(journeyId, journey);
        }

        [HttpDelete("{journeyId}")]
        public void DeleteJourney(int journeyId)
        {
           _journeyService.DeleteJourney(journeyId);
        }
    }
}