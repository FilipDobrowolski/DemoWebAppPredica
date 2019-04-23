using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoAppPredica.Models.Models.Journeys;
using DemoAppPredica.Processing.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAppPredica.Api.Controllers
{
    [Authorize]
    [Route("api/journeys")]
    [ApiController]
    public class JourneysController : ControllerBase
    {
        public readonly IJourneyService _journeyService;

        public JourneysController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpGet]
        public IActionResult GetAllJourneys()
        {
            var journeys = _journeyService.GetAllJourneys();
            return Ok(journeys);
        }

        [HttpGet("id")]
        public IActionResult GetJourneyById(int journeyId)
        {
            var journey = _journeyService.GetJourneyById(journeyId);
            return Ok(journey);
        }

        [HttpGet("user")]
        public List<Journey> GetUserJourneys(Guid userId)
        {
            return _journeyService.GetUserJourneys(userId);
        }

        [HttpPost]
        public void CreateJourney([FromBody] Journey journey)
        {
            _journeyService.CreateJourney(journey);
        }

        [HttpPut]
        public void UpdateJourney([FromBody] Journey journey)
        {
            _journeyService.UpdateJourney(journey);
        }

        [HttpDelete("{journeyId}")]
        public void DeleteJourney(int journeyId)
        {
            _journeyService.DeleteJourney(journeyId);
        }
    }
}