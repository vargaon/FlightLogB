namespace FlightLogNet.Repositories.Entities
{
    public class Person
    {
        public long Id { get; set; }

        public PersonType PersonType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address Address { get; set; }

        public long MemberId { get; set; }
    }
}