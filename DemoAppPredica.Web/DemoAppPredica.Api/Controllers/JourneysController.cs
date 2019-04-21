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
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var journeys = _journeyService.GetAllJourneys();
            return Ok(journeys);
        }

        [HttpGet("user/{userId}")]
        public List<Journey> GetUserJourneys(Guid userId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _journeyService.GetUserJourneys(userId);
        }

        [HttpPost]
        public void CreateJourney([FromBody] Journey journey)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _journeyService.CreateJourney(journey);
        }

        [HttpPut("{journeyId}")]
        public void UpdateJourney(int journeyId, [FromBody] Journey journey)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _journeyService.UpdateJourney(journeyId, journey);
        }

        [HttpDelete("{journeyId}")]
        public void DeleteJourney(int journeyId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _journeyService.DeleteJourney(journeyId);
        }
    }
}