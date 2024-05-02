namespace FlightLogNet.Repositories.Entities
{
    public class ClubAirplane
    {
        public long Id { get; set; }

        public string Immatriculation { get; internal set; }

        public AirplaneType AirplaneType { get; internal set; }

        public bool Archive { get; set; }
    }
}