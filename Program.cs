using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; //added
using SnakeClasses; //added

namespace Snake3
{
    class Program
    {
        static Random randy = new Random();
        static void Main(string[] args)
        {
            #region Setup
            Console.WindowHeight = 25;
            Console.BufferHeight = 26;
            Console.WindowWidth = 65;
            Console.BufferWidth = 66;
            int right = 0;
            int left = 1;
            int down = 2;
            int up = 3;                    
            Vector[] directions = new Vector[]
            {
                new Vector(1, 0), //right
                new Vector(-1, 0), //left
                new Vector(0, 1), //down
                new Vector(0, -1), //up
            };
            do
            {
                int sleepTime = 100;
                int direction = right;
                StartScreen();
                Console.ReadKey();
                Console.Clear();
                short playerScore = 0;
                List<Vector> obstacles = new List<Vector>()
            {
                new Vector(randy.Next(1, Console.WindowWidth), randy.Next(2, Console.WindowHeight)),
                new Vector(randy.Next(1, Console.WindowWidth), randy.Next(2, Console.WindowHeight)),
                new Vector(randy.Next(1, Console.WindowWidth), randy.Next(2, Console.WindowHeight)),
                new Vector(randy.Next(1, Console.WindowWidth), randy.Next(2, Console.WindowHeight)),
                new Vector(randy.Next(1, Console.WindowWidth), randy.Next(2, Console.WindowHeight)),
                new Vector(randy.Next(1, Console.WindowWidth), randy.Next(2, Console.WindowHeight)),
            };
                foreach (Vector v in obstacles)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(v.X, v.Y);
                    Console.Write('X');
                }//end foreach

                Queue<Vector> snakeElements = new Queue<Vector>();
                for (int i = 0; i <= 5; i++)
                {
                    snakeElements.Enqueue(new Vector(0, i));
                }//end snakeElements

                Vector food;
                do
                {
                    food = new Vector(randy.Next(0, Console.WindowWidth), randy.Next(3, Console.WindowHeight));
                } while (snakeElements.Contains(food) || obstacles.Contains(food));
                Console.SetCursorPosition(food.X, food.Y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write('0');
                #endregion

                #region Game Loop
                bool gameLoop = true;
                while (gameLoop == true)
                {
                    //Move
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo userInput = Console.ReadKey();
                        if (userInput.Key == ConsoleKey.LeftArrow || userInput.Key == ConsoleKey.A)
                        {
                            if (direction != right) direction = left;
                        }//end if left
                        if (userInput.Key == ConsoleKey.RightArrow || userInput.Key == ConsoleKey.D)
                        {
                            if (direction != left) direction = right;
                        }//end if right
                        if (userInput.Key == ConsoleKey.UpArrow || userInput.Key == ConsoleKey.W)
                        {
                            if (direction != down) direction = up;
                        }//end if up
                        if (userInput.Key == ConsoleKey.DownArrow || userInput.Key == ConsoleKey.S)
                        {
                            if (direction != up) direction = down;
                        }//end if down                   
                    }//end if Console.keyAvailable
                    //Balance movement speed since cols are larger than rows
                    if(direction == up || direction == down)
                    {
                        sleepTime = 125;
                    }
                    else
                    {
                        sleepTime = 100;
                    }
                    //Generate snake
                    Vector snakeHead = snakeElements.Last();
                    Vector nextDirection = directions[direction];
                    Vector newSnakeHead = new Vector(snakeHead.X + nextDirection.X, snakeHead.Y + nextDirection.Y);
                    Console.SetCursorPosition(snakeHead.X, snakeHead.Y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('*');

                    //Detect if out of Bounds
                    if (newSnakeHead.X == 0) { gameLoop = false; }
                    if (newSnakeHead.X >= 65) { gameLoop = false; }
                    if (newSnakeHead.Y == 2) { gameLoop = false; }
                    if (newSnakeHead.Y >= 25) { gameLoop = false; }
                    //Detect if Snake runs into self before adding new head to Queue
                    foreach (Vector v in snakeElements)
                    {
                        if (v.X == newSnakeHead.X && v.Y == newSnakeHead.Y) gameLoop = false;                       
                    }                    
                    //Add new snake head
                    snakeElements.Enqueue(newSnakeHead);
                    Console.SetCursorPosition(newSnakeHead.X, newSnakeHead.Y);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (direction == right) Console.Write('>');
                    if (direction == left) Console.Write('<');
                    if (direction == down) Console.Write('V');
                    if (direction == up) Console.Write('^');
                    
                    //Detect if player scores, generate new food and new obstacle
                    if (newSnakeHead.X == food.X && newSnakeHead.Y == food.Y)
                    {
                        do
                        {
                            food = new Vector(randy.Next(0, Console.WindowWidth), randy.Next(4, Console.WindowHeight));
                        } while (snakeElements.Contains(food) || obstacles.Contains(food));

                        Console.SetCursorPosition(food.X, food.Y);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('0');
                        sleepTime-= 2;
                        playerScore++;

                        Vector obstacle = new Vector();
                        do
                        {
                            obstacle = new Vector(randy.Next(0, Console.WindowWidth), randy.Next(3, Console.WindowHeight));
                        } while (snakeElements.Contains(obstacle) || obstacles.Contains(obstacle) || (food.X != obstacle.X) && (food.Y != obstacle.Y));
                        obstacles.Add(obstacle);
                        Console.SetCursorPosition(obstacle.X, obstacle.Y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('X');                        
                    }//end if
                    else
                    {
                        Vector last = snakeElements.Dequeue();
                        Console.SetCursorPosition(last.X, last.Y);
                        Console.Write(' ');
                    }//end else
                    DisplayScore(sleepTime, playerScore);                    
                    
                    //Obstacle detection                    
                    foreach (Vector v in obstacles)
                    {
                        if (v.X == newSnakeHead.X && v.Y == newSnakeHead.Y)
                        {
                            gameLoop = false;
                        }
                    }
                    //Detect if player eats self written during Snake Generation                 
                }//end while gameloop
                #endregion
                GameOver(playerScore);
            } while (true);
        }//end Main()

        static void StartScreen()
        {
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  Welcome to my C# Snake Game!\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  You can use either the arrow keys or AWSD to move the snake.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  \n  Eat eggs (0) to grow and score more points!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  \n  If you run into obstacles (X) or the walls it's game over!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  Oh yeah, be careful not to eat yourself (*) either!");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\n  Press any key to begin.");
        }//end StartScreen()
        static void GameOver(int score)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  You died! Your score was:" + score + "\n\n  Would you like to try again?");
            Console.SetCursorPosition(0, 5);
            string replay = Console.ReadLine().ToUpper();
            if (replay == string.Empty) { }
            else if (replay.Substring(0, 1) == "N")
            {
                Console.Clear();
                Environment.Exit(10);
            }
            Console.Clear();
        }
        static void DisplayScore(int sleepTime, int playerScore)
        {
            Console.SetCursorPosition(0, 0);
            Thread.Sleep(sleepTime);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Your current score is: {0}", playerScore);
            Console.WriteLine("_________________________________________________________________");
            Console.SetCursorPosition(0, 0);
        }
    }//end class
}//end namespace
