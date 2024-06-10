using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedyAirLy
{
    class Order
    {
        public string OrderId { get; set; }
        public string Destination { get; set; }
        public bool Scheduled { get; set; } = false;
    }
}
