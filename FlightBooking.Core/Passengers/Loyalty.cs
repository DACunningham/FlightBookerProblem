namespace FlightBooking.Core.Passengers
{
    public class Loyalty : Passenger
    {
        public int LoyaltyPoints { get; set; }
        public bool IsUsingLoyaltyPoints { get; set; }

        public Loyalty()
        {
            AllowedBags = 2;
            RouteBasePriceWeight = 1;
        }
    }
}
