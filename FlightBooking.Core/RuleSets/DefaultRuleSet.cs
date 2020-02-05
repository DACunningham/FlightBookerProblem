namespace FlightBooking.Core.RuleSets
{
    public class DefaultRuleSet : IRuleSet
    {
        public double Profit { get; set; }
        public int PassengerCount { get; set; }
        public int SeatCount { get; set; }
        public double MinPercentage { get; set; }

        public virtual bool CanProceed()
        {
            if (Profit > 0
                && PassengersNotExceedSeats()
                && MinimumNumPassengersReached())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PassengersNotExceedSeats()
        {
            return (SeatCount - PassengerCount) >= 0 ? true : false;
        }

        private bool MinimumNumPassengersReached()
        {
            return (PassengerCount / (double)SeatCount) > MinPercentage ? true : false;
        }
    }
}
