using System;
using System.Collections.Generic;
using System.Text;

namespace FlightBooking.Core.Passengers
{
    public class Discounted : Passenger
    {
        public Discounted()
        {
            AllowedBags = 0;
            RouteBasePriceWeight = 0.5;
        }
    }
}
