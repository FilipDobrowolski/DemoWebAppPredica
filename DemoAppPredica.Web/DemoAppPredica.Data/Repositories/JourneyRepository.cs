using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoAppPredica.Data.Interfaces;
using DemoAppPredica.Models.Models.Journeys;
using Microsoft.EntityFrameworkCore;

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
            _context.Journeys.Attach(journeyToDelete);
            _context.SaveChanges();
        }

        public IEnumerable<Journey> GetAllJourneys()
        {
            return _context.Journeys;
        }

        public Journey GetJourneyById(int journeyId)
        {
            return _context.Journeys.FirstOrDefault(journey => journey.Id == journeyId);
        }

        public IEnumerable<Journey> GetUserJourneys(Guid userId)
        {
            return _context.Journeys.Where(journey => journey.UserId == userId);
        }

        public void UpdateJourney(Journey journey)
        {
            _context.Journeys.Attach(journey);
            _context.Entry(journey).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
