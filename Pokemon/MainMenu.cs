using System;
using System.Collections.Generic;
using System.Text;

public class MainMenu
{
    enum Options {GamePlay = 1, Exit}
    Options option;
    public MainMenu()
    {

    }

    public void Menu()
    {
        Console.WriteLine("Pokemon League - Console edition");
        Console.WriteLine("1 - Jugar 2- Salir");
        option = (Options)Convert.ToInt32(Console.ReadLine());
        switch(option)
        {
            case Options.GamePlay:
                Game.GetInstance().status = Game.Status.GamePlay;
                break;
            case Options.Exit:
                Game.GetInstance().status = Game.Status.Exit;
                break;
            default:
                Console.WriteLine("Opción no válida");
                break;
        }
        Console.Clear();
    }
}

