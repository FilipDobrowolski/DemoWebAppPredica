using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using DemoAppPredica.Models.Models.Journeys;

namespace DemoAppPredica.Data.Interfaces
{
    public interface IJourneyRepository
    {
        IEnumerable<Journey> GetAllJourneys();
        IEnumerable<Journey> GetUserJourneys(Guid userId);
        Journey GetJourneyById(int journeyId);
        void CreateJourney(Journey journey);
        void UpdateJourney(Journey journey);
        void DeleteJourney(int journeyId);
        
    }
}
