using bs.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(55,55);
            GameboardController GameboardController = new GameboardController();
            GameboardController.Render();
            Console.ReadLine();
        }
    }
}
