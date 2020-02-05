namespace FlightBooking.Core.Passengers
{
    public abstract class Passenger
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int AllowedBags { get; protected set; }
        public double RouteBasePriceWeight { get; protected set; }
    }
}
