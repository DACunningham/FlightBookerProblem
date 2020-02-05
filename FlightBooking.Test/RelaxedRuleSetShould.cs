using FlightBooking.Core.RuleSets;
using FluentAssertions;
using Xunit;

namespace FlightBooking.Test
{
    public class RelaxedRuleSetShould
    {
        private readonly RelaxedRuleSet sut;

        public RelaxedRuleSetShould()
        {
            sut = new RelaxedRuleSet
            {
                SeatCount = 10,
                MinPercentage = 0.5
            };
        }

        [Theory]
        [InlineData(1, 1, 0, false)]
        [InlineData(11, 1, 1000, false)]
        [InlineData(0, 5, 0, false)]
        [InlineData(0, 4, 0, false)]
        [InlineData(0, 6, 0, true)]
        [InlineData(6, 0, 600, true)]
        public void ReturnBoolIfFlightCanProceed(int passengerCount, int airlinePassengerCount, int profit, bool result)
        {
            // Arrange
            sut.AirlinePassengerCount = airlinePassengerCount;
            sut.PassengerCount = passengerCount;
            sut.Profit = profit;

            // Act
            bool canProceed = sut.CanProceed();

            // Assert
            canProceed.Should().Be(result);
        }
    }
}
