using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedyAirLy
{
    class Flight
    {
        public int FlightNumber { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public int Day { get; set; }
        public List<string> Orders { get; set; }

        public Flight(int flightNumber, string departureCity, string arrivalCity, int day)
        {
            FlightNumber = flightNumber;
            DepartureCity = departureCity;
            ArrivalCity = arrivalCity;
            Day = day;
            Orders = new List<string>();
        }

        public override string ToString()
        {
            return $"Flight: {FlightNumber}, departure: {DepartureCity}, arrival: {ArrivalCity}, day: {Day}";
        }

        public string DetailedInfo()
        {
            return $"{this} -> Orders: {string.Join(", ", Orders)}";
        }
    }
}
