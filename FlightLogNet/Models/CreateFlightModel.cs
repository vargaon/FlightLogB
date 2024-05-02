namespace FlightLogNet.Models
{
    using System;

    public class CreateFlightModel
    {
        public long AirplaneId { get; set; }

        public long PilotId { get; set; }

        public long? CopilotId { get; set; }

        public DateTime TakeOffTime { get; internal set; }

        public string Task { get; internal set; }

        public FlightType Type { get; internal set; }
    }
}
