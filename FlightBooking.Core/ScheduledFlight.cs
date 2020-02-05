using FlightBooking.Core.Passengers;
using FlightBooking.Core.RuleSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightBooking.Core
{
    public class ScheduledFlight
    {
        public ScheduledFlight(FlightRoute flightRoute)
        {
            FlightRoute = flightRoute;
            Passengers = new List<Passenger>();
            RuleSet = new DefaultRuleSet();
            BackupPlanes = new List<Plane>
                {
                new Plane()
                {
                    Id = 456,
                    Name = "Bombardier Q400",
                    NumberOfSeats = 15
                },
                new Plane()
                {
                    Id = 789,
                    Name = "ATR 640",
                    NumberOfSeats = 30
                }
            };
        }

        public FlightRoute FlightRoute { get; }
        public Plane Aircraft { get; private set; }
        public ICollection<Plane> BackupPlanes { get; set; }
        public List<Passenger> Passengers { get; }
        public IRuleSet RuleSet { get; set; }
        public int LoyaltyPointsRedeemed { get; set; }
        public int LoyaltyPointsAccrued { get; set; }
        public double Revenue { get; set; }
        public double Costs { get; set; }

        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        public void SetAircraftForRoute(Plane aircraft)
        {
            Aircraft = aircraft;
        }

        /// <summary>
        /// Generates the full details of the current scheduled flight.
        /// </summary>
        /// <returns><c>string</c> representation of flight information</returns>
        /// <exception cref="ArgumentNullException">Thrown if a correct cast of RuleSet cannot be compoleted</exception>
        public string GetSummary()
        {
            foreach (var passenger in Passengers.OfType<Loyalty>())
            {
                if (passenger.IsUsingLoyaltyPoints)
                {
                    CalculateLoyaltyRedeemed(passenger);
                }
                else
                {
                    CalculateLoyaltyAccrued(passenger);
                }
            }

            CalculateRevenue();
            CalculateCosts();

            if (RuleSet is RelaxedRuleSet relaxedRuleSet)
            {
                relaxedRuleSet.MinPercentage = FlightRoute.MinimumTakeOffPercentage;
                relaxedRuleSet.PassengerCount = Passengers.Count;
                relaxedRuleSet.AirlinePassengerCount = Passengers.OfType<Airline>().Count();
                relaxedRuleSet.Profit = CalculateProfit();
                relaxedRuleSet.SeatCount = Aircraft.NumberOfSeats;
                return BuildSummaryString(relaxedRuleSet);
            }
            else if (RuleSet is DefaultRuleSet defaultRuleSet)
            {
                defaultRuleSet.MinPercentage = FlightRoute.MinimumTakeOffPercentage;
                defaultRuleSet.PassengerCount = Passengers.Count;
                defaultRuleSet.Profit = CalculateProfit();
                defaultRuleSet.SeatCount = Aircraft.NumberOfSeats;
                return BuildSummaryString(defaultRuleSet);
            }
            else
            {
                throw new ArgumentNullException("RuleSet", "RuleSet is not set to a known type.");
            }
        }

        /// <summary>
        /// Builds summary string from constituent parts
        /// </summary>
        /// <param name="ruleSet"><c>IRuleSet</c> will determine what rules are to be applied to the flight</param>
        private string BuildSummaryString(IRuleSet ruleSet)
        {
            StringBuilder result = new StringBuilder();

            if (Passengers.Count > Aircraft.NumberOfSeats)
            {
                OtherAircraft(ref result);

                return result.ToString();
            }

            result.Append("Flight summary for ");
            result.Append(FlightRoute.Title);

            AddVerticalWhiteSpace(ref result);

            AddPassengersDetails(ref result);

            AddVerticalWhiteSpace(ref result);

            AddCostInfo(ref result);

            AddVerticalWhiteSpace(ref result);

            AddLoyaltyInfo(ref result);

            AddVerticalWhiteSpace(ref result);

            if (ruleSet.CanProceed())
            {
                result.Append("THIS FLIGHT MAY PROCEED");
            }
            else
            {
                result.Append("FLIGHT MAY NOT PROCEED");
            }
            return result.ToString();
        }

        /// <summary>
        /// Generates infomration about flight passengers
        /// </summary>
        /// <param name="result"><c>StringBuilder</c> with text to be displayed</param>
        private void AddPassengersDetails(ref StringBuilder result)
        {
            result.Append("Total passengers: ");
            result.Append(Passengers.Count);

            AddNewLine(ref result);
            AddIndentation(ref result);

            result.Append("General sales: ");
            result.Append(Passengers.OfType<General>().Count());

            AddNewLine(ref result);
            AddIndentation(ref result);

            result.Append("Loyalty member sales: ");
            result.Append(Passengers.OfType<Loyalty>().Count());

            AddNewLine(ref result);
            AddIndentation(ref result);

            result.Append("Airline employee comps: ");
            result.Append(Passengers.OfType<Airline>().Count());

            AddNewLine(ref result);
            AddIndentation(ref result);

            result.Append("Discounted sales: ");
            result.Append(Passengers.OfType<Discounted>().Count());

            AddVerticalWhiteSpace(ref result);
            result.Append("Total expected baggage: ");

            int baggageCount = 0;
            foreach (var passenger in Passengers)
            {
                baggageCount += passenger.AllowedBags;
            }

            result.Append(baggageCount);
        }

        /// <summary>
        /// Generates infomration about flight costs
        /// </summary>
        /// <param name="result"><c>StringBuilder</c> with text to be displayed</param>
        private void AddCostInfo(ref StringBuilder result)
        {
            result.Append("Total revenue from flight: ");

            result.Append(Revenue);

            AddNewLine(ref result);

            result.Append("Total costs from flight: ");

            result.Append(Costs);

            AddNewLine(ref result);

            var profit = CalculateProfit();

            result.Append((profit > 0 ? "Flight generating profit of: " : "Flight losing money of: "));
            result.Append(profit);
        }

        private double CalculateProfit()
        {
            return Revenue - Costs;
        }

        /// <summary>
        /// Calculate total revenue associated with flight and store it in object property <c>Revenue</c>
        /// </summary>
        private void CalculateRevenue()
        {
            foreach (var passenger in Passengers)
            {
                if (passenger is Loyalty loyaltyPassenger && loyaltyPassenger.IsUsingLoyaltyPoints)
                {
                    // Do not add basePrice for this passenger as using loyalty points
                }
                else
                {
                    Revenue += passenger.RouteBasePriceWeight * FlightRoute.BasePrice;
                }

            }
        }

        /// <summary>
        /// Calculate total costs associated with flight and store it in object property <c>Costs</c>
        /// </summary>
        private void CalculateCosts()
        {
            Costs += Passengers.Count * FlightRoute.BaseCost;
        }

        /// <summary>
        /// Generates infomration about flight loyalty points
        /// </summary>
        /// <param name="result"><c>StringBuilder</c> with text to be displayed</param>
        private void AddLoyaltyInfo(ref StringBuilder result)
        {
            result.Append("Total loyalty points given away: ");
            result.Append(LoyaltyPointsAccrued);
            AddNewLine(ref result);
            result.Append("Total loyalty points redeemed: ");
            result.Append(LoyaltyPointsRedeemed);
            AddNewLine(ref result);
        }

        private void OtherAircraft(ref StringBuilder result)
        {
            result.Append("THIS FLIGHT MAY NOT PROCEED.");
            AddNewLine(ref result);
            result.Append("Other more suitable aircraft are:");
            AddNewLine(ref result);
            int count = 0;

            foreach (var plane in BackupPlanes)
            {
                if (Passengers.Count <= plane.NumberOfSeats)
                {
                    result.Append(plane.Name);
                    result.Append(" could handle this flight.");
                    AddNewLine(ref result);
                    count++;
                }
            }

            if (count == 0)
            {
                result.Append("No other aircraft in your inventory can accept this flight.");
            }
        }

        private void CalculateLoyaltyRedeemed(Loyalty passenger)
        {
            var loyaltyPointsRedeemed = Convert.ToInt32(Math.Ceiling(FlightRoute.BasePrice));
            passenger.LoyaltyPoints -= loyaltyPointsRedeemed;
            LoyaltyPointsRedeemed += loyaltyPointsRedeemed;
        }

        private void CalculateLoyaltyAccrued(Loyalty passenger)
        {
            passenger.LoyaltyPoints += FlightRoute.LoyaltyPointsGained;
            LoyaltyPointsAccrued += FlightRoute.LoyaltyPointsGained;
        }

        private void AddNewLine(ref StringBuilder sb)
        {
            sb.Append(Environment.NewLine);
        }

        private void AddVerticalWhiteSpace(ref StringBuilder sb)
        {
            sb.Append(Environment.NewLine + Environment.NewLine);
        }

        private void AddIndentation(ref StringBuilder sb)
        {
            sb.Append("    ");
        }
    }
}
