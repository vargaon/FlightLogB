namespace FlightLogNet.Repositories.Entities
{
    public class Airplane
    {
        public long Id { get; set; }

        public ClubAirplane ClubAirplane { get; set; }

        public string GuestAirplaneImmatriculation { get; set; }

        public string GuestAirplaneType { get; set; }
    }
}