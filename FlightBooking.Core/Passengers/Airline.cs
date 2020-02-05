using System;
using System.Collections.Generic;
using System.Text;

namespace FlightBooking.Core.Passengers
{
    public class Airline : Passenger
    {
        public Airline()
        {
            AllowedBags = 1;
            RouteBasePriceWeight = 0;
        }
    }
}
