using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


public abstract class Trainer
{
    public string name;
    public List<Pokemon> pokemonList;
    public int remainingPokemons;
    public int battlePokemon;    

    public Trainer()
    {

    }
    public Trainer(string name, string path)
    {
        this.name = name;
        pokemonList = new List<Pokemon>();
        SetPokemons(path);
        remainingPokemons = pokemonList.Count;
    }
    public virtual void SetPokemons(string path)
    {
        string[] playerPokemons = File.ReadAllLines(path);
        for (int i = 0; i < playerPokemons.Length; i++)
        {
            pokemonList.Add(new Pokemon(name + "/" + playerPokemons[i]));
        }
    }
}

