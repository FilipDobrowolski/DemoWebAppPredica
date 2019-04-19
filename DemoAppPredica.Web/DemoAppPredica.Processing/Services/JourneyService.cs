using System;
using System.Collections.Generic;
using System.Text;
using DemoAppPredica.Models.Models.Journeys;
using DemoAppPredica.Processing.Interfaces;

namespace DemoAppPredica.Processing.Services
{
    public class JourneyService : IJourneyService
    {
        public void CreateJourney(Journey journey)
        {
            throw new NotImplementedException();
        }

        public void DeleteJourney(int journeyId)
        {
            throw new NotImplementedException();
        }

        public List<Journey> GetAllJourneys()
        {
            throw new NotImplementedException();
        }

        public List<Journey> GetUserJourneys(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateJourney(int journeyId, Journey journey)
        {
            throw new NotImplementedException();
        }
    }
}
