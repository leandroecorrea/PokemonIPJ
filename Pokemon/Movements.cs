using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


public class Movements
{    
    public string name;
    public int attack;
    public int heal;
    public int PP;
    public int maxPP;
    public ElementalTypes type;
    public enum MovementType { Damage, Heal, Mix, None}
    public enum DamageClass {Melee, Magic }
    public MovementType movementType;
    public DamageClass damageClass;
    public float movementEffectiveness;

    public Movements()
    {

    }
    public virtual void Use()
    {

    }
    public virtual Pokemon Use(Pokemon pokemon)
    {
        return pokemon;
    }
    //public virtual int StatusTurns()
    //{
    //    return 0;
    //}    
    public virtual KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
        PP--;
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }   
    public int ConfusionTurns()
    {
        Random random = new Random();
        return random.Next(2, 5);
    }
}

public class Struggle : Movements, INonTrainableMove
{
    public Struggle()
    {
        name = "Struggle";
        attack = 50;
        heal = 0;
        PP = 10;              
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override void Use()
    {
        Console.WriteLine("El movimiento " + name + "fue usado");                
    }
}

public class WingAttack : Movements
{
    public WingAttack()
    {
        name = "Wing Attack";
        attack = 60;
        heal = 0;
        PP = 35;
        maxPP = 35;
        type = ElementalTypes.Flying;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
        PP--;
        if(foePokemon.GetNonVolatileStatus() == NonVolatileStatus.Burn)
        {
            foePokemon.SetNonVolatileStatus(NonVolatileStatus.None);
        }
        movementPokemon.AddVolatileStatus(VolatileStatus.Charge);
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
}

public class Confused : Movements, INonTrainableMove
{
    public Confused()
    {
        name = "Autolesión";
        attack = 40;
        heal = 0;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override Pokemon Use(Pokemon pokemon)
    {
        Console.WriteLine("El movimiento " + name + "fue usado contra sí mismo");
        return pokemon;
    }
    //public int SetStatusTurns()
    //{
    //    Random random = new Random();
    //    return random.Next(2, 5);
    //}
    
}

public class MirrorMove : Movements
{
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        movementPokemon.movementChosen = foePokemon.movementChosen;
        Console.WriteLine("El pokemon ha imitado el movimiento " + name + "fue usado");
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
}

public class Reflect : Movements, ITurnLimitedEffect, ICauseVStatus
{
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + movementPokemon.movementChosen.name + "fue usado. Su defensa aumentó!");
        PP--;
        if ((movementPokemon.GetVolatileStatus() | VolatileStatus.Reflect) == VolatileStatus.Reflect)
        {
            Console.WriteLine("El pokemon ya se encuentra afectado, nada sucedió.");
        }
        else
        {
            movementPokemon.AddVolatileStatus(VolatileStatus.Reflect);
            movementPokemon.battleDefense *= 2;
        }        
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public int SetStatusTurns()
    {
        Console.WriteLine("El reflejo durará por cinco turnos");
        return 5;
    }
    public VolatileStatus StatusCaused()
    {        
        return VolatileStatus.Reflect;
    }
}

public class SkyAttack : Movements, ITurnLimitedEffect, ICauseVStatus
{
    public SkyAttack()
    {
        name = "Sky Attack";
        attack = 140;
        heal = 0;
        PP = 5;
        maxPP = 5;
        type = ElementalTypes.Flying;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;        
    }
    
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        movementPokemon.AddVolatileStatus(StatusCaused());
        base.Use(movementPokemon, foePokemon);
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public int SetStatusTurns()
    {
        Console.WriteLine("El pokemon está cargando su ataque en el cielo");
        return 1;
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.Reflect;
    }
}

public class WhirlWind : Movements
{
    public WhirlWind()
    {
        name = "Whirlwind";
        attack = 0;
        heal = 0;
        PP = 20;
        maxPP = 20;
        type = ElementalTypes.Normal;
        movementType = MovementType.None;
        damageClass = DamageClass.Melee;
    }
    //esta clase debería poder acceder al switchpokemon del rival
}

public class Psybeam : Movements
{
    public Psybeam()
    {
        name = "Psybeam";
        attack = 65;
        heal = 0;
        PP = 20;
        maxPP = 20;
        type = ElementalTypes.Psychic;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Magic;
    }
    //este movimiento puede provocar confusión en el rival, hay que hacer un checkstatus antes de cada jugada
    // el ch
}

public class Psychic : Movements, ICauseVStatus, ITurnLimitedEffect
{
    public Psychic()
    {
        name = "Psychic";
        attack = 120;
        heal = 0;
        PP = 10;
        maxPP = 10;
        type = ElementalTypes.Psychic;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Magic;
    }

    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        foePokemon.AddVolatileStatus(StatusCaused());
        Console.WriteLine("El movimiento " + name + "fue usado");
        PP--;
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public int SetStatusTurns()
    {
        return ConfusionTurns();
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.Confusion;
    }
}

public class Recover : Movements
{
    public Recover()
    {
        name = "Recover";
        attack = 0;
        heal = 30;
        PP = 10;
        maxPP = 10;
        type = ElementalTypes.Normal;
        movementType = MovementType.Heal;
        damageClass = DamageClass.Magic;
    }    
}


class MissingMovementException : Exception
{
    public MissingMovementException(string errorMessage) : base(errorMessage)
    {
    }

    public Movements GetDefaultMovement()
    {
        return new Struggle();
    }
}

public static class MovementDatabase
{
    public static Movements GetMovement(string movementName)
    {
        try
        {
            switch (movementName)
            {
                case "Wing Attack":
                    return new WingAttack();
                case "Sky Attack":
                    return new SkyAttack();                    
                case "Whirlwind":
                    return new WhirlWind();
                case "Psybeam":
                    return new Psybeam();
                case "Psychic":
                    return new Psychic();
                case "Recover":
                    return new Recover();
                default:
                    throw new MissingMovementException("El movimiento con nombre" + movementName + " no existe en la base de datos de movimientos");
            }
        }
        catch (MissingMovementException e)
        {
            Console.WriteLine(e.Message);
            return e.GetDefaultMovement();
        }
    }
}
