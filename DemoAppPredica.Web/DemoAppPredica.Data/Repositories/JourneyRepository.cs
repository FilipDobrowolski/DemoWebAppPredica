using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoAppPredica.Data.Interfaces;
using DemoAppPredica.Models.Models.Journeys;

namespace DemoAppPredica.Data.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly CustomDbContext _context;

        public JourneyRepository(CustomDbContext context)
        {
            _context = context;
        }
        public void CreateJourney(Journey journey)
        {
            _context.Journeys.Add(journey);
        }

        public void DeleteJourney(int journeyId)
        {
            Journey journeyToDelete = _context.Journeys.First(journey => journey.Id == journeyId);
            journeyToDelete.IsValid = false;
            _context.Journeys.Update(journeyToDelete);
        }

        public IEnumerable<Journey> GetAllJourneys()
        {
            return _context.Journeys;
        }

        public IEnumerable<Journey> GetUserJourneys(Guid userId)
        {
            return _context.Journeys.Where(journey => journey.UserId == userId);
        }

        public void UpdateJourney(int journeyId, Journey journey)
        {
            _context.Journeys.Update(journey);
        }
    }
}
