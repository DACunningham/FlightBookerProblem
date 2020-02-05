namespace FlightBooking.Core.RuleSets
{
    public class RelaxedRuleSet : DefaultRuleSet, IRuleSet
    {
        public int AirlinePassengerCount { get; set; }

        public override bool CanProceed()
        {
            if (PassengersNotExceedSeats()
                && MinimumNumAirlinePassengersReached())
            {
                return true;
            }

            return base.CanProceed();
        }

        private bool PassengersNotExceedSeats()
        {
            return (SeatCount - PassengerCount) >= 0 ? true : false;
        }

        private bool MinimumNumAirlinePassengersReached()
        {
            return (AirlinePassengerCount / (double)SeatCount) > MinPercentage ? true : false;
        }
    }
}
