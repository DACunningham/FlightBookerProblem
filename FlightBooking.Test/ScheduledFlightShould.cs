using FlightBooking.Core;
using FlightBooking.Core.Passengers;
using FlightBooking.Core.RuleSets;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace FlightBooking.Test
{
    public class ScheduledFlightShould
    {
        private readonly FlightRoute londonToParis;
        private readonly ScheduledFlight sut;

        public ScheduledFlightShould()
        {
            londonToParis = new FlightRoute("London", "Paris")
            {
                BaseCost = 50,
                BasePrice = 100,
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };

            sut = new ScheduledFlight(londonToParis);
            sut.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 12 });
        }

        [Fact]
        public void AllowFlightWhenAirlinePassengersMoreThanMinPercentage()
        {
            // Arrange
            ScheduledFlight sut = new ScheduledFlight(londonToParis);
            sut.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 3 });
            sut.AddPassenger(new Airline
            {
                Name = "Trevor",
                Age = 47,
            });
            sut.AddPassenger(new Airline
            {
                Name = "Trevor",
                Age = 47,
            });
            sut.AddPassenger(new Airline
            {
                Name = "Trevor",
                Age = 47,
            });
            sut.RuleSet = new RelaxedRuleSet();


            // Act
            string result = sut.GetSummary();

            // Assert
            result.Should().Be(AirlineStaffOnlyCorrectString());
        }

        private string AirlineStaffOnlyCorrectString()
        {
            string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
            string _newLine = Environment.NewLine;
            string Indentation = "    ";
            string result = "Flight summary for " + londonToParis.Title;

            result += _verticalWhiteSpace;

            result += "Total passengers: " + 3;
            result += _newLine;
            result += Indentation + "General sales: " + 0;
            result += _newLine;
            result += Indentation + "Loyalty member sales: " + 0;
            result += _newLine;
            result += Indentation + "Airline employee comps: " + 3;
            result += _newLine;
            result += Indentation + "Discounted sales: " + 0;

            result += _verticalWhiteSpace;
            result += "Total expected baggage: " + 3;

            result += _verticalWhiteSpace;

            result += "Total revenue from flight: " + 0;
            result += _newLine;
            result += "Total costs from flight: " + 150;
            result += _newLine;

            var profitSurplus = 0 - 150;

            result += (profitSurplus > 0 ? "Flight generating profit of: " : "Flight losing money of: ") + profitSurplus;

            result += _verticalWhiteSpace;

            result += "Total loyalty points given away: " + 0 + _newLine;
            result += "Total loyalty points redeemed: " + 0 + _newLine;

            result += _verticalWhiteSpace;
            result += "THIS FLIGHT MAY PROCEED";

            return result;
        }

        [Fact]
        public void ReturnSameStringAsBeforeCodeRefactor()
        {
            // Arrange
            sut.AddPassenger(new General
            {
                Name = "Steve",
                Age = 30
            });
            sut.AddPassenger(new General
            {
                Name = "Mark",
                Age = 12
            });
            sut.AddPassenger(new General
            {
                Name = "James",
                Age = 36
            });
            sut.AddPassenger(new General
            {
                Name = "Jane",
                Age = 32
            });
            sut.AddPassenger(new Loyalty
            {
                Name = "John",
                Age = 29,
                LoyaltyPoints = 1000,
                IsUsingLoyaltyPoints = true,
            });
            sut.AddPassenger(new Loyalty
            {
                Name = "Sarah",
                Age = 45,
                LoyaltyPoints = 1250,
                IsUsingLoyaltyPoints = false,
            });
            sut.AddPassenger(new Loyalty
            {
                Name = "Jack",
                Age = 60,
                LoyaltyPoints = 50,
                IsUsingLoyaltyPoints = false,
            });
            sut.AddPassenger(new Airline
            {
                Name = "Trevor",
                Age = 47,
            });
            sut.AddPassenger(new General
            {
                Name = "Alan",
                Age = 34
            });
            sut.AddPassenger(new General
            {
                Name = "Suzy",
                Age = 21
            });
            sut.AddPassenger(new Discounted
            {
                Name = "Dexter",
                Age = 22
            });

            // Act
            string summary = sut.GetSummary();

            // Assert
            summary.Should().Be(GenerateOriginalCorrectString());
        }

        private string GenerateOriginalCorrectString()
        {
            string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
            string _newLine = Environment.NewLine;
            string Indentation = "    ";
            string result = "Flight summary for " + londonToParis.Title;

            result += _verticalWhiteSpace;

            result += "Total passengers: " + 11;
            result += _newLine;
            result += Indentation + "General sales: " + 6;
            result += _newLine;
            result += Indentation + "Loyalty member sales: " + 3;
            result += _newLine;
            result += Indentation + "Airline employee comps: " + 1;
            result += _newLine;
            result += Indentation + "Discounted sales: " + 1;

            result += _verticalWhiteSpace;
            result += "Total expected baggage: " + 13;

            result += _verticalWhiteSpace;

            result += "Total revenue from flight: " + 850;
            result += _newLine;
            result += "Total costs from flight: " + 550;
            result += _newLine;

            var profitSurplus = 850 - 550;

            result += (profitSurplus > 0 ? "Flight generating profit of: " : "Flight losing money of: ") + profitSurplus;

            result += _verticalWhiteSpace;

            result += "Total loyalty points given away: " + 10 + _newLine;
            result += "Total loyalty points redeemed: " + 100 + _newLine;

            result += _verticalWhiteSpace;
            result += "THIS FLIGHT MAY PROCEED";

            return result;
        }

        [Fact]
        public void ReturnCorrectSummary()
        {
            // Arrange
            List<Passenger> passengers = (List<Passenger>)CreatePassengerList();
            foreach (var passenger in passengers)
            {
                sut.AddPassenger(passenger);
            }

            // Act
            string summary = sut.GetSummary();

            // Assert
            summary.Should().Be(GenerateNewCorrectString());
        }

        private string GenerateNewCorrectString()
        {
            string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
            string _newLine = Environment.NewLine;
            string Indentation = "    ";
            string result = "Flight summary for " + londonToParis.Title;

            result += _verticalWhiteSpace;

            result += "Total passengers: " + 5;
            result += _newLine;
            result += Indentation + "General sales: " + 1;
            result += _newLine;
            result += Indentation + "Loyalty member sales: " + 2;
            result += _newLine;
            result += Indentation + "Airline employee comps: " + 1;
            result += _newLine;
            result += Indentation + "Discounted sales: " + 1;

            result += _verticalWhiteSpace;
            result += "Total expected baggage: " + 6;

            result += _verticalWhiteSpace;

            result += "Total revenue from flight: " + 250;
            result += _newLine;
            result += "Total costs from flight: " + 250;
            result += _newLine;

            var profitSurplus = 250 - 250;

            result += (profitSurplus > 0 ? "Flight generating profit of: " : "Flight losing money of: ") + profitSurplus;

            result += _verticalWhiteSpace;

            result += "Total loyalty points given away: " + 5 + _newLine;
            result += "Total loyalty points redeemed: " + 100 + _newLine;

            result += _verticalWhiteSpace;
            result += "FLIGHT MAY NOT PROCEED";

            return result;
        }

        #region CreatePassengerList
        private static ICollection<Passenger> CreatePassengerList()
        {
            ICollection<Passenger> passengers = new List<Passenger>
            {
                new General
                {
                    Name = "Dexter",
                    Age = 30,
                },
                new Airline
                {
                    Name = "Mary",
                    Age = 20,
                },
                new Loyalty
                {
                    Name = "Jim",
                    Age = 40,
                    IsUsingLoyaltyPoints = true,
                    LoyaltyPoints = 1000
                },
                new Loyalty
                {
                    Name = "John",
                    Age = 45,
                    IsUsingLoyaltyPoints = false,
                    LoyaltyPoints = 200
                },
                new Discounted
                {
                    Name = "Dexter",
                    Age = 22
                }
            };

            return passengers;
        }
        #endregion

        [Fact]
        public void ReturnListOfOtherAircraftWhenTooManyPassengers()
        {
            // Arrange
            List<Passenger> passengers = (List<Passenger>)CreatePassengerList();
            foreach (var passenger in passengers)
            {
                sut.AddPassenger(passenger);
            }
            foreach (var passenger in passengers)
            {
                sut.AddPassenger(passenger);
            }
            foreach (var passenger in passengers)
            {
                sut.AddPassenger(passenger);
            }

            // Act
            string summary = sut.GetSummary();

            // Assert
            summary.Should().Be(GenerateTooManyPassengersError());
        }

        private string GenerateTooManyPassengersError()
        {
            string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
            string _newLine = Environment.NewLine;
            //string Indentation = "    ";
            string result = "THIS FLIGHT MAY NOT PROCEED.";
            result += _newLine;

            result += "Other more suitable aircraft are:";
            result += _newLine;
            result += "Bombardier Q400 could handle this flight.";
            result += _newLine;
            result += "ATR 640 could handle this flight.";
            result += _newLine;

            return result;
        }
    }
}
