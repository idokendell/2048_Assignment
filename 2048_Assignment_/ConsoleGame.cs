using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Assignment_
{
    public class ConsoleGame
    {
        private static Dictionary<ConsoleKey, Direction> _moves = new Dictionary<ConsoleKey, Direction>() {
                                                                                                { ConsoleKey.UpArrow,Direction.UP},
                                                                                                { ConsoleKey.DownArrow, Direction.DOWN },
                                                                                                { ConsoleKey.LeftArrow, Direction.LEFT },
                                                                                                { ConsoleKey.RightArrow, Direction.RIGHT } };
        private Board Game;
        public ConsoleGame()
        {
            Game = new Board();
            Console.WriteLine("Good Luck:");
            StartGame();
        }
        public void StartGame()
        {
            while (Game.Status==GameStatus.Idle)
            {
                PrettyPrint();
                ConsoleKey move = Console.ReadKey().Key;
                Game.Move(_moves[move]);
            }
            PrettyPrint();
            if (Game.Status==GameStatus.Win) Console.WriteLine("U Won!!");
            else if (Game.Status==GameStatus.Lose)
            {
                Console.WriteLine("you lost!");
            }
            Console.WriteLine("press any key to start over");
            Console.ReadKey();
            Game.Start();
            StartGame();

        }
        private void PrettyPrint()
        {
            Console.WriteLine("==================================================");
            for (int i = 0; i<Game.Data.GetLength(0); i++)
            {
                Console.WriteLine();
                Console.Write("|");
                for (int j = 0; j< Game.Data.GetLength(1); j++)
                {
                    {
                        if (Game.Data[i, j]==0) Console.Write("    |");
                        else
                        {
                            Console.ForegroundColor=GetNumberColor(Game.Data[i, j]);
                            Console.Write($"{Game.Data[i, j]}{String.Concat(Enumerable.Repeat(" ", 4-(""+Game.Data[i, j]).Length))}");
                            Console.ForegroundColor=ConsoleColor.White;
                            Console.Write("|");
                        }
                    }
                    //Array.ForEach(row, x => Console.Write(((x!=0)? ""+x:" ")+"|" ));
                }
            }
            Console.WriteLine();
            Console.WriteLine("==================================================");
            Console.WriteLine($"your score:{Game.Points}");
        }
        private static ConsoleColor GetNumberColor(int num)
        {
            switch (num)
            {
                case 0:
                    return ConsoleColor.DarkGray;
                case 2:
                    return ConsoleColor.Cyan;
                case 4:
                    return ConsoleColor.Magenta;
                case 8:
                    return ConsoleColor.Red;
                case 16:
                    return ConsoleColor.Green;
                case 32:
                    return ConsoleColor.Yellow;
                case 64:
                    return ConsoleColor.Yellow;
                case 128:
                    return ConsoleColor.DarkCyan;
                case 256:
                    return ConsoleColor.Cyan;
                case 512:
                    return ConsoleColor.DarkMagenta;
                case 1024:
                    return ConsoleColor.Magenta;
                default:
                    return ConsoleColor.Red;
            }
        }
    }
}
