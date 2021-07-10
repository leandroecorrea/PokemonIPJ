﻿using System;
using System.Collections.Generic;
using System.Text;


public class GamePlay
{
    public Player player;
    public Liga liga;
    public bool gamePlayLoop;
    string playerName;
    string optionFilePath;
    enum GamePlayOptions { NewGame, LoadGame, Error }

    public GamePlay()
    { 
        gamePlayLoop = true;        
    }    

    public void Play()
    {
        GamePlaySwitcher();
        while (gamePlayLoop)
        {
            gamePlayLoop = liga.Lobby(player);
        }
    }
    public void GamePlaySwitcher()
    {
        GamePlayOptions option = GamePlayOptions.Error;
        while (option >= GamePlayOptions.Error || option < GamePlayOptions.NewGame)
        {
            Console.WriteLine("1- Nueva partida   2 - Cargar partida guardada");
            option = (GamePlayOptions)Convert.ToInt32(Console.ReadLine());
            switch (option)
            {
                case GamePlayOptions.NewGame:
                    StartNewGame();
                    break;
                case GamePlayOptions.LoadGame:
                    StartNewGame();
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
        }
    }
    public void StartNewGame()
    {
        Console.WriteLine("Ingresá tu nombre");
        playerName = Console.ReadLine();
        Console.WriteLine("Bienvenido, " + playerName);
        //int option = Convert.ToInt32(Console.ReadLine());
        //switch (option)
        //{
        //    case 1:
        //        Console.WriteLine("Elegiste un combo de pokemones balanceados, suerte!");
        //        optionFilePath = "Player/player.txt";
        //        break;
        //    default:
        //        Console.WriteLine("Ingresaste una opción no válida. Para no hacer muchos while loops, te asignaré un equipo de pokemones balanceados");
        //        optionFilePath = "Player/player.txt";
        //        break;
        //}
        player = new Player(playerName, "Player/player.txt");
        Console.WriteLine(playerName + ", tus pokemones serán los siguientes:");
        for(int i = 0; i < player.pokemonList.Count; i++)
        {
            Console.WriteLine(player.pokemonList[i].name);
        }
        Console.WriteLine("Presione cualquier tecla para continuar");
        Console.ReadLine();
        liga = new Liga(player);
    }
}

