using System;
using System.Collections.Generic;
using System.Text;

namespace FlightBooking.Core.Passengers
{
    public class General : Passenger
    {
        public General()
        {
            AllowedBags = 1;
            RouteBasePriceWeight = 1;
        }
    }
}
