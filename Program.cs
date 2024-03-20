using OpenTK;
using Proiect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficaCalculator
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
           
            GameWindow window = new GameWindow(800, 400);
            Game gm = new Game(window);
        }
    }
}
