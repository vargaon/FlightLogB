namespace FlightLogNet.Integration
{
    using System.Collections.Generic;

    public class ClubUser
    {
        public long MemberId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<string> Roles { get; set; }
    }
}