using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SpeedyAirLy
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a flight schedule
            List<Flight> flightSchedule = CreateFlightSchedule();

            // Step 2: Display the flight schedule (User Story #1)
            DisplayFlightSchedule(flightSchedule);

            // Step 3: Load orders from the JSON file
            string filePath = @"D:\\Castro\\Development\\SpeedyAir_Solution\\SpeedyAir_Solution\\data\\coding-assigment-orders.json";
            List<Order> orders = LoadOrdersFromJson(filePath);

            // Step 4: Allocate orders to flights based on destination and capacity
            AllocateOrdersToFlights(flightSchedule, orders);

            // Step 5: Display the flight itineraries with allocated orders (User Story #2)
            DisplayOrderAllocations(orders, flightSchedule);
        }

        static List<Flight> CreateFlightSchedule()
        {
            // Create a list to hold the flight schedule
            return new List<Flight>
            {
                new Flight(1, "YUL", "YYZ", 1),
                new Flight(2, "YUL", "YYC", 1),
                new Flight(3, "YUL", "YVR", 1),
                new Flight(4, "YUL", "YYZ", 2),
                new Flight(5, "YUL", "YYC", 2),
                new Flight(6, "YUL", "YVR", 2)
            };
        }

        static void DisplayFlightSchedule(List<Flight> flightSchedule)
        {
            Console.WriteLine("Flight Schedule:");
            foreach (var flight in flightSchedule)
            {
                Console.WriteLine(flight);
            }
            Console.WriteLine(); // Blank line for readability
        }

        static List<Order> LoadOrdersFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var rawOrders = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

            List<Order> orders = new List<Order>();
            foreach (var item in rawOrders)
            {
                orders.Add(new Order { OrderId = item.Key, Destination = item.Value["destination"] });
            }

            return orders;
        }

        static void AllocateOrdersToFlights(List<Flight> flightSchedule, List<Order> orders)
        {
            Dictionary<string, List<Flight>> destinationFlights = new Dictionary<string, List<Flight>>();

            // Group flights by destination
            foreach (var flight in flightSchedule)
            {
                if (!destinationFlights.ContainsKey(flight.ArrivalCity))
                {
                    destinationFlights[flight.ArrivalCity] = new List<Flight>();
                }
                destinationFlights[flight.ArrivalCity].Add(flight);
            }

            // Distribute orders to the respective flights based on priority order
            foreach (var order in orders)
            {
                if (destinationFlights.ContainsKey(order.Destination))
                {
                    foreach (var flight in destinationFlights[order.Destination])
                    {
                        if (flight.Orders.Count < 20)
                        {
                            flight.Orders.Add(order.OrderId);
                            order.Scheduled = true;
                            break;
                        }
                    }
                }
            }
        }

        static void DisplayOrderAllocations(List<Order> orders, List<Flight> flightSchedule)
        {
            Console.WriteLine("Order Allocations:");

            // Create a mapping of flight number to flight for quick lookup
            Dictionary<string, (int flightNumber, string departure, string arrival, int day)> orderFlightMapping = new Dictionary<string, (int, string, string, int)>();
            foreach (var flight in flightSchedule)
            {
                foreach (var orderId in flight.Orders)
                {
                    orderFlightMapping[orderId] = (flight.FlightNumber, flight.DepartureCity, flight.ArrivalCity, flight.Day);
                }
            }

            // Display each order's allocation
            foreach (var order in orders)
            {
                if (order.Scheduled && orderFlightMapping.ContainsKey(order.OrderId))
                {
                    var flightInfo = orderFlightMapping[order.OrderId];
                    Console.WriteLine($"order: {order.OrderId}, flightNumber: {flightInfo.flightNumber}, departure: {flightInfo.departure}, arrival: {flightInfo.arrival}, day: {flightInfo.day}");
                }
                else
                {
                    Console.WriteLine($"order: {order.OrderId}, flightNumber: not scheduled");
                }
            }
        }
    }
}
