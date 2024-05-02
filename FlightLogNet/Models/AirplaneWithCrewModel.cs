namespace FlightLogNet.Models
{
    public class AirplaneWithCrewModel
    {
        public AirplaneModel Airplane { get; set; }

        public PersonModel Pilot { get; set; }

        public PersonModel Copilot { get; set; }

        public string Note { get; set; }
    }
}
