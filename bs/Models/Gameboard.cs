using bs.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs_Models
{
    public class GameBoard
    {
        public Gamefield[] Gamefields { get; set; }
        public List<Ship> Ships { get; set; } = new List<Ship>();
        private Random Random = new Random();
        private int BoardWidth;
        public Gamefield this[(int X, int Y) position]
        {
            get => Gamefields[position.Y * BoardWidth + position.X];
            set => Gamefields[position.Y * BoardWidth + position.X] = value;
        }

        public GameBoard(int boardWidth)
        {
            Gamefields = new Gamefield[boardWidth * boardWidth];
            BoardWidth = boardWidth;
            for (int index = 0; index < Gamefields.Length; index++)
                Gamefields[index] = Gamefield.Empty;
            CreateShips();
            PlaceShips();
        }

        private void CreateShips()
        {
            void createShip(int length, int amount)
            {
                for (int index = 1; index <= amount; index++)
                    Ships.Add(new Ship(length));
            }
            createShip(5, 1);
            createShip(4, 2);
            createShip(3, 3);
            createShip(2, 4);
        }

        private bool InBounds(int currentIndex, bool horizontal, int length) =>
            currentIndex >= 0
            && currentIndex < Gamefields.Length
            && (horizontal ? ConvertTo2D(currentIndex).X + length <= BoardWidth : ConvertTo2D(currentIndex).Y + length <= BoardWidth);


        private void PlaceShips()
        {
            //TODO: after some ship placements it is impossible to fill the whole board, have to restart the process then
            foreach (Ship ship in Ships)
            {
                bool horizontal = Random.Next(2) == 0 ? false : true;
                int currentIndex = Random.Next(Gamefields.Length);

                //while to get valid coordinates to place the ship
                while (!ValidPlacement(ship, ConvertTo2D(currentIndex), horizontal))
                {
                    currentIndex = Random.Next(Gamefields.Length);
                    horizontal = Random.Next(2) == 0 ? false : true;
                }

                //placing individual ship  parts for its length && saving ships coordinates
                for (int index = 0; index < ship.Length; index++)
                {
                    var realIndex = horizontal ? currentIndex + index : currentIndex + index * BoardWidth;

                    Gamefields[realIndex] = Gamefield.Ship;
                    ship.ShipsCoordinates.Add(ConvertTo2D(realIndex));
                }
            }
        }

        private bool ValidPlacement(Ship ship, (int X, int Y) startIndex, bool horizontal)
        {
            //checks all 8 fields around the current field + current field, returns false if any field contains a ship
            bool xyCheck((int X, int Y) currentIndex, int length)
            {
                int rangeX = 2;
                int startX = -1;
                int rangeY = 2;
                int startY = -1;

                //setting different start and range values for x and y if they are at the boarder of the gameboard.
                if (currentIndex.X == BoardWidth - 1)
                    rangeX = 1;
                if (currentIndex.X == 0)
                    startX = 0;
                if (currentIndex.Y == BoardWidth - 1)
                    rangeY = 1;
                if (currentIndex.Y == 0)
                    startY = 0;

                // [-1,-1] [0,-1] [1,-1]
                // [-1, 0] [0, 0] [1, 0]
                // [-1, 1] [0, 1] [1, 1]
                for (int xIndex = startX; xIndex < rangeX; xIndex++)
                    for (int yIndex = startY; yIndex < rangeY; yIndex++)
                    {
                        var pos1D = (xIndex + currentIndex.X + (yIndex + currentIndex.Y) * BoardWidth);
                        if (!InBounds(pos1D, horizontal, length) || Gamefields[pos1D] != Gamefield.Empty)
                            return false;
                    }
                return true;
            }
            //loop over ships length, returns false if any position the ship should be placed on already contains a ship
            for (int shipPart = 0; shipPart < ship.Length; shipPart++)
                if ((horizontal && !xyCheck((startIndex.X + shipPart, startIndex.Y), ship.Length))
                    || (!horizontal && !xyCheck((startIndex.X, startIndex.Y + shipPart), ship.Length)))
                    return false;

            return true;
        }

        public (int X, int Y) ConvertTo2D(int coordinates) => (coordinates % BoardWidth, coordinates / BoardWidth);

    }
}
