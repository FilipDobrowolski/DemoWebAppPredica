using System;
using System.Collections.Generic;
using System.Text;

namespace DemoAppPredica.Models.Models.Journeys
{
    public class Journey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DestinationCountry { get; set; }
        public int Cost { get; set; }
        public bool IsValid { get; set; }
        public Guid? UserId { get; set; }

    }
}
