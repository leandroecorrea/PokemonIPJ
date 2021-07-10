using System;
using System.Collections.Generic;
using System.Text;


public class Game
{
    MainMenu mainMenu;
    GamePlay gamePlay;
    public enum Status { Menu, GamePlay, Exit}    
    public Status status = Status.Menu;
    private static Game instance;    

    public static Game GetInstance()
    {
        if(instance == null)
        {
            instance = new Game();
        }
        return instance;
    }

    private Game()
    {
        mainMenu = new MainMenu();
        gamePlay = new GamePlay();
    }

    public bool Start()
    {
        bool gameLoop = true;        
        switch (status)
        {
            case Status.GamePlay:
                gamePlay.Play();
                break;
            case Status.Menu:
                mainMenu.Menu();
                break;
            case Status.Exit:
                gameLoop = false;
                break;
            default:
                break;
        }
        return gameLoop;
    }
}
