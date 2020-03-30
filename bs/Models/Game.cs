using bs_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Models
{
    public class Game
    {
        public GameBoard GameBoardPlayer { get; set; }
        public GameBoard GameBoardPlayerAI { get; set; }
        CurrentPlayer CurrentPlayer { get; set; } = CurrentPlayer.Player;
        int Boardwidth;
        Random Random = new Random();


        public Game(int width)
        {
            GameBoardPlayer = new GameBoard(width);
            GameBoardPlayerAI = new GameBoard(width);
            Boardwidth = width;
        }

        public bool TryShoot((int X, int Y) coordinates)
        {
            var gameboard = CurrentPlayer == CurrentPlayer.Player ? GameBoardPlayerAI : GameBoardPlayer;
            var field = gameboard[coordinates];

            // change value of gamefield if it is hit
            if (field != Gamefield.HitEmpty && field != Gamefield.HitShip)
            {
                switch (gameboard[coordinates])
                {
                    case Gamefield.Empty:
                        gameboard[coordinates] = Gamefield.HitEmpty;
                        break;
                    case Gamefield.Ship:
                        gameboard[coordinates] = Gamefield.HitShip;
                        var ship = gameboard.Ships
                            .Where(s => s.ShipsCoordinates.Contains((coordinates.X, coordinates.Y)))
                            .FirstOrDefault();
                        ship.ShipHitCounter++;
                        break;
                }

                //change player
                CurrentPlayer = CurrentPlayer == CurrentPlayer.AI ? CurrentPlayer.Player : CurrentPlayer.AI;
                if (CurrentPlayer == CurrentPlayer.AI)
                    TryShoot((Random.Next(Boardwidth), Random.Next(Boardwidth)));
                return true;
            }
            return false;
        }



    }
}
