namespace FlightLogNet.Models
{
    using System;

    public class FlightModel
    {
        public long Id { get; set; }

        public DateTime TakeoffTime { get; set; }

        public DateTime? LandingTime { get; set; }

        public AirplaneModel Airplane { get; set; }

        public PersonModel Pilot { get; set; }

        public PersonModel Copilot { get; set; }

        public string Task { get; set; }
    }
}
