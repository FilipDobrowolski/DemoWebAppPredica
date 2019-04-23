using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoAppPredica.Data.Interfaces;
using DemoAppPredica.Models.Models.Journeys;
using DemoAppPredica.Processing.Interfaces;

namespace DemoAppPredica.Processing.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;

        public JourneyService(IJourneyRepository journeyRepository)
        {
            _journeyRepository = journeyRepository;
        }

        public Journey GetJourneyById(int journeyId)
        {
            return _journeyRepository.GetJourneyById(journeyId);
        }

        public void CreateJourney(Journey journey)
        {
            _journeyRepository.CreateJourney(journey);
        }

        public void DeleteJourney(int journeyId)
        {
            _journeyRepository.DeleteJourney(journeyId);
        }

        public List<Journey> GetAllJourneys()
        {
            return _journeyRepository.GetAllJourneys().ToList();
        }

        public List<Journey> GetUserJourneys(Guid userId)
        {
            return _journeyRepository.GetUserJourneys(userId).ToList();
        }

        public void UpdateJourney(Journey journey)
        {
            _journeyRepository.UpdateJourney(journey);
        }
    }
}
