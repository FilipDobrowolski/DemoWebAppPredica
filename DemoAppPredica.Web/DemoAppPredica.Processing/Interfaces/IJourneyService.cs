using DemoAppPredica.Models.Models.Journeys;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoAppPredica.Processing.Interfaces
{
    public interface IJourneyService
    {

        List<Journey> GetAllJourneys();
        List<Journey> GetUserJourneys(Guid userId);
        void CreateJourney(Journey journey);
        void UpdateJourney(int journeyId, Journey journey);
        void DeleteJourney(int journeyId);

    }
}
