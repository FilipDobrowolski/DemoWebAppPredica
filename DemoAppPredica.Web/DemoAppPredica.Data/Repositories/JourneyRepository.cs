using System;
using System.Collections.Generic;
using System.Text;
using DemoAppPredica.Data.Interfaces;
using DemoAppPredica.Models.Models.Journeys;

namespace DemoAppPredica.Data.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
        public void CreateJourney(Journey journey)
        {
            throw new NotImplementedException();
        }

        public void DeleteJourney(int journeyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journey> GetAllJourneys()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Journey> GetUserJourneys(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateJourney(int journeyId, Journey journey)
        {
            throw new NotImplementedException();
        }
    }
}
