using FlightBooking.Core.RuleSets;
using FluentAssertions;
using Xunit;

namespace FlightBooking.Test
{
    public class DefaultRuleSetShould
    {
        private readonly DefaultRuleSet sut;

        public DefaultRuleSetShould()
        {
            sut = new DefaultRuleSet
            {
                SeatCount = 10,
                MinPercentage = 0.5
            };
        }

        [Theory]
        [InlineData(1, 100, false)]
        [InlineData(11, 1100, false)]
        [InlineData(5, 500, false)]
        [InlineData(4, 400, false)]
        [InlineData(0, 0, false)]
        [InlineData(6, 600, true)]
        [InlineData(6, -20, false)]
        public void ReturnBoolIfFlightCanProceed(int passengerCount, int profit, bool result)
        {
            // Arrange
            sut.PassengerCount = passengerCount;
            sut.Profit = profit;

            // Act
            bool canProceed = sut.CanProceed();

            // Assert
            canProceed.Should().Be(result);
        }
    }
}
