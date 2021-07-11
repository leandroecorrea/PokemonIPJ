using System;
using System.Collections.Generic;
using System.Text;


public interface ITurnLimitedEffect 
{
    public int SetStatusTurns();
}
public interface ICauseNVStatus
{
    public KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon);
    public NonVolatileStatus StatusCaused();
}
public interface ICauseVStatus
{
    public KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon);
    public VolatileStatus StatusCaused();
}
public interface INonTrainableMove
{

}
public interface IDamageBoth
{
    public int AutoDamage(int damage);
}


public interface IMultipleAttack
{
    public int AttackTimes();
}