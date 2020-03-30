using bs.Models;
using bs_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Views
{
    public class GameboardController
    {
        public Game Game { get; set; }
        private readonly int Width = 10;
        public GameboardController()
        {
            Game = new Game(Width);
        }

        public void Render()
        {
            while (Game.GameBoardPlayer.Gamefields.Any(s => s == Gamefield.Ship) && Game.GameBoardPlayerAI.Gamefields.Any(s => s == Gamefield.Ship))
            {
                Console.Clear();

                RenderBoard(false, Game.GameBoardPlayer.Gamefields);
                Console.WriteLine();
                RenderBoard(true, Game.GameBoardPlayerAI.Gamefields);
                Console.WriteLine();
                RenderGameLoop();
            }
            //win/lose condition, if there are still "unhit" ships in: 
            //'your gameboard =>  you win 
            //'enemy board' => you yose 
            if (Game.GameBoardPlayer.Gamefields.Any(s => s == Gamefield.Ship))
                Console.WriteLine("You win :)");
            else if (Game.GameBoardPlayerAI.Gamefields.Any(s => s == Gamefield.Ship))
                Console.WriteLine("You lost :(");
        }

        private void RenderGameLoop()
        {
            int xOut = default;
            int yOut = default;
            string inputX = default;
            string inputY = default;
            bool firstrun = true;

            //if there are no more ships in either gameboard, the game ends
            bool gameNotOver() =>
                Game.GameBoardPlayer.Gamefields.Any(s => s == Gamefield.Ship)
                && Game.GameBoardPlayerAI.Gamefields.Any(s => s == Gamefield.Ship);

            //run at least once
            //repeat while shot hits already hit field and game is still ongoing
            do
            {
                //if its not the first run, something went wrong
                if (!firstrun)
                    Console.WriteLine("You already hit this field");

                //input for X coordinate
                Console.WriteLine("Please type in the column you want to shoot at.");
                inputX = Console.ReadLine();
                //if parse fails, input is no int or is not in bounds of the gameboard
                while (!ParseStringYToInt(inputX, out xOut) || xOut > Width - 1)
                {
                    Console.WriteLine("Something went wrong, please reenter the coordinate for the column.");
                    inputX = Console.ReadLine();
                }

                //input for Y coordinate
                Console.WriteLine("Please type in the row you want to shoot at.");
                inputY = Console.ReadLine();
                //if parse fails, input is no int or is not in bounds of the gameboard
                while (!int.TryParse(inputY, out yOut) || yOut > Width || yOut <= 0)
                {
                    Console.WriteLine("Something went wrong, please reenter the coordinate for the row.");
                    inputY = Console.ReadLine();
                }
                firstrun = false;
                //continue while the shot hits a field that was hit before
            } while (!Game.TryShoot((xOut, yOut - 1)) && gameNotOver());


        }

        private void RenderBoard(bool AI, Gamefield[] field)
        {
            //Coordinates: A-Z
            if (AI)
            {
                Console.Write("   ");
                for (int index = 0; index < Width; index++)
                {
                    Console.Write((char)(index + 65));
                    if (index % Width != Width - 1)
                        Console.Write("|");
                    else
                        Console.WriteLine();
                }
            }

            //gameboard
            if (AI)
                Console.Write("01|");
            for (int index = 0; index < field.Length; index++)
            {
                char output = SpriteSwitch(index, field, AI);
                if (index % Width == Width - 1 && index != 0)
                {
                    Console.ForegroundColor = ColorSwitch(AI, field, index);
                    Console.WriteLine(output);
                    if (index < field.Length - Width && AI)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("   ");
                        for (int rowIndex = 0; rowIndex < Width - 1; rowIndex++)
                            Console.Write("-+");
                        Console.Write("-");
                        Console.WriteLine();
                        Console.ResetColor();
                        string linenumber = ((index / Width) + 2).ToString("D2");
                        Console.Write($"{linenumber}|");
                    }
                }
                else
                {
                    Console.ForegroundColor = ColorSwitch(AI, field, index);
                    Console.Write(output);
                    if (AI)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("|");
                    }
                }
                Console.ResetColor();
            }
        }

        private char SpriteSwitch(int index, Gamefield[] fields, bool AI)
        {
            return fields[index] switch
            {
                Gamefield.Empty => '#',
                Gamefield.HitEmpty => 'O',
                Gamefield.Ship => AI ? '#' : 'X',
                Gamefield.HitShip => 'X',
                _ => 'e'
            };
        }

        private ConsoleColor ColorSwitch(bool AI, Gamefield[] field, int index)
        {
            return field[index] switch
            {
                Gamefield.Empty => ConsoleColor.Blue,
                Gamefield.HitEmpty => ConsoleColor.DarkYellow,
                Gamefield.Ship => AI ? ConsoleColor.Blue : ConsoleColor.Green,
                Gamefield.HitShip => ConsoleColor.Red,
                _ => ConsoleColor.Magenta,
            };
        }

        public static bool ParseStringYToInt(string input, out int output)
        {
            input = input.ToUpper();
            var c = input[input.Length - 1];
            if (c < 'A' || c > 'Z')
            {
                output = 9999;
                return false;
            }
            output = c - 65;
            return true;
        }
    }
}
