using System;
using System.Collections.Generic;
using System.Threading;

// dwarf is (0)
// constant game speed -> Thread.Sleep(150);

struct Dwarf
{
    public int x;
    public int y;
    public string str;
    public ConsoleColor color;
}
struct Rock
{
    public int x;
    public int y;
    public char c; //public int width; //1-3 symbols
    public ConsoleColor color;
}
class FallingRocks
{
    static void PrintOnPos(int x, int y, char ch, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(ch);
    }
    static void PrintStringOnPos(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }
    static void Main(string[] args)
    {
        char[] arrRocks = { '^', '@', '*', '&', '+', '%', '$', '#', '!', '.', ';' };
        int playFieldWidth, livesCount, score;
        Console.BufferHeight = Console.WindowHeight = 30; //rows
        Console.BufferWidth = Console.WindowWidth = 30; //columns
        Dwarf userDwarf = new Dwarf();
        List<Rock> rocks = new List<Rock>();
        Random rndGenerator = new Random();
    newGameLabel:
        playFieldWidth = 15;
        livesCount = 3;
        score = 0;
        userDwarf.x = playFieldWidth / 2 - 1;
        userDwarf.y = Console.WindowHeight - 1;
        userDwarf.str = "(0)";
        userDwarf.color = ConsoleColor.White;

        while (true)
        {
            {
                Rock newRock = new Rock();
                switch (rndGenerator.Next(0, 14))
                {
                    case 1: newRock.color = ConsoleColor.Blue; break;
                    case 2: newRock.color = ConsoleColor.Cyan; break;
                    case 3: newRock.color = ConsoleColor.DarkBlue; break;
                    case 4: newRock.color = ConsoleColor.DarkCyan; break;
                    case 5: newRock.color = ConsoleColor.DarkGray; break;
                    case 6: newRock.color = ConsoleColor.DarkGreen; break;
                    case 7: newRock.color = ConsoleColor.DarkMagenta; break;
                    case 8: newRock.color = ConsoleColor.DarkRed; break;
                    case 9: newRock.color = ConsoleColor.DarkYellow; break;
                    case 10: newRock.color = ConsoleColor.Gray; break;
                    case 11: newRock.color = ConsoleColor.Green; break;
                    case 12: newRock.color = ConsoleColor.Magenta; break;
                    case 13: newRock.color = ConsoleColor.Red; break;
                    case 14: newRock.color = ConsoleColor.Yellow; break;
                }

                newRock.x = rndGenerator.Next(0, playFieldWidth);
                newRock.y = 0;
                newRock.c = arrRocks[rndGenerator.Next(0, 11)];
                rocks.Add(newRock);
            }
            // Move dwarf (keypressed)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                while (Console.KeyAvailable) Console.ReadKey(true);
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if (userDwarf.x - 1 >= 0)
                    {
                        userDwarf.x -= 1;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (userDwarf.x + 3 < playFieldWidth)
                    {
                        userDwarf.x += 1;
                    }
                }
            }
            List<Rock> newList = new List<Rock>();
            // Move Rocks
            for (int i = 0; i < rocks.Count; i++)
            {
                Rock oldRock = rocks[i];
                Rock newRock = new Rock();
                newRock.x = oldRock.x;
                newRock.y = oldRock.y + 1;
                newRock.c = oldRock.c;
                newRock.color = oldRock.color;
                // Check for collision
                if (newRock.y == userDwarf.y && ((newRock.x == userDwarf.x) || (newRock.x == userDwarf.x + 1) || (newRock.x == userDwarf.x + 2)))
                {
                    livesCount--;
                    Console.Beep(1455, 200);
                    PrintOnPos(newRock.x, newRock.y, 'X', ConsoleColor.Red);
                    PrintStringOnPos(playFieldWidth + 2, 6, "Press Key...", ConsoleColor.Red);
                    if (livesCount != 0)
                    {
                        Console.ReadKey();
                    }
                    rocks.Clear();
                    if (livesCount <= 0)
                    {
                        PrintStringOnPos(playFieldWidth + 2, 0, "Lives: " + livesCount, ConsoleColor.Green);
                        PrintStringOnPos(playFieldWidth + 2, 4, "GAME OVER !", ConsoleColor.Red);
                        PrintStringOnPos(playFieldWidth + 2, 6, "Press [Enter]", ConsoleColor.Red);
                        PrintStringOnPos(playFieldWidth + 2, 8, "to exit ...", ConsoleColor.Red);
                        PrintStringOnPos(playFieldWidth + 2, 18, "Press [Space]", ConsoleColor.Red);
                        PrintStringOnPos(playFieldWidth + 2, 20, "for new game", ConsoleColor.Red);
                        do
                        {
                            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                            while (Console.KeyAvailable) Console.ReadKey(true);
                            if (pressedKey.Key == ConsoleKey.Enter)
                            {
                                return;
                            }
                            else if (pressedKey.Key == ConsoleKey.Spacebar)
                            {
                                rocks.Clear();
                                goto newGameLabel;
                            }
                        }
                        while (true);
                        //Environment.Exit(0); 
                    }
                }
                if (newRock.y < Console.WindowHeight)
                {
                    newList.Add(newRock);
                }
            }
            rocks = newList;

            // Clear the console
            Console.Clear();
            // Redraw all
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                PrintOnPos(playFieldWidth, i, '|', ConsoleColor.Green);
            }
            PrintStringOnPos(userDwarf.x, userDwarf.y, userDwarf.str, userDwarf.color);
            foreach (Rock rock in rocks)
            {
                PrintOnPos(rock.x, rock.y, rock.c, rock.color);
            }
            // Draw Score/Points
            score++;
            PrintStringOnPos(playFieldWidth + 2, 0, "Lives: " + livesCount, ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, 1, "Score: " + score, ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, 11, "FALLING", ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, 13, "    ROCKS ", ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, 15, "       GAME ", ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, Console.WindowHeight - 4, "Made by:", ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, Console.WindowHeight - 3, "   Software", ConsoleColor.Green);
            PrintStringOnPos(playFieldWidth + 2, Console.WindowHeight - 2, "     Academy", ConsoleColor.Green);
            // Slow down -> Sleep
            Thread.Sleep(150);
        }
    }
}



