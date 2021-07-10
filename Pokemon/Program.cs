using System;


class Program
{
    static void Main(string[] args)
    {
        bool gameLoop = true;
        Game game = Game.GetInstance();
        while (gameLoop)
        {
            gameLoop = game.Start();
        }
    }
}

