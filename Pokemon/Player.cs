using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


public class Player : Trainer
{
    public Player(string name, string path) : base(name, path)
    {
        
    }

    public override void SetPokemons(string path)
    {
        string[] playerPokemons = File.ReadAllLines(path);
        for (int i = 0; i < playerPokemons.Length; i++)
        {
            pokemonList.Add(new Pokemon("Player/" + playerPokemons[i]));
        }
    }

}
