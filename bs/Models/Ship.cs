using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs_Models
{
    public class Ship
    {
        public int Length { get; }
        public List<(int X, int Y)> ShipsCoordinates { get; set; } = new List<(int X, int Y)>();
        public int ShipHitCounter { get; set; } = 0;
        public bool ShipIsDestroyed => ShipHitCounter == Length;

        public Ship(int length)
        {
            Length = length;
        }
    }
}
