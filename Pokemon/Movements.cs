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
    public enum MovementType { Damage, Heal, DamageHeal, DamageBoth, IgnoreArmor, AffectsTrainer, MultipleAttack, OneHitKO, Counter, AutoDamage, None }
    public enum DamageClass { Melee, Magic }
    public MovementType movementType;
    public DamageClass damageClass;
    public float movementEffectiveness;

    public Movements()
    {
        PP = 35;
        maxPP = 35;
    }
    public virtual void Use()
    {

    }
    public virtual Pokemon Use(Pokemon pokemon)
    {
        return pokemon;
    }
    public virtual KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
        PP--;
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public virtual KeyValuePair<Trainer, Trainer> Use(Trainer caster, Trainer foe)
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
        PP--;
        return new KeyValuePair<Trainer, Trainer>(caster, caster);
    }
    public int ConfusionTurns()
    {
        Random random = new Random();
        return random.Next(2, 5);
    }
    public int ChargeTurns()
    {
        return 1;
    }
    public virtual bool MovementAccuracy(Trainer caster, Trainer foe)
    {
        return true;
    }
}

public class Struggle : Movements, INonTrainableMove, IDamageBoth
{
    public Struggle()
    {
        name = "Struggle";
        attack = 50;
        heal = 0;
        PP = 10;
        type = ElementalTypes.Normal;
        movementType = MovementType.DamageBoth;
        damageClass = DamageClass.Melee;
    }
    public override void Use()
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
    }
    public int AutoDamage(int damage)
    {
        return Convert.ToInt32(damage * 0.4);
    }
}

public class DragonRage : Movements
{
    public DragonRage()
    {
        name = "Dragon Rage";
        attack = 40;
        heal = 0;
        PP = 10;
        type = ElementalTypes.Normal;
        movementType = MovementType.IgnoreArmor;
        damageClass = DamageClass.Melee;
    }
}

public class Rage : Movements, ICauseVStatus
{
    public Rage()
    {
        name = "Rage";
        attack = 20;
        heal = 0;
        PP = 20;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }

    public override KeyValuePair<Trainer, Trainer> Use(Trainer caster, Trainer foe)
    {
        caster.pokemonList[caster.battlePokemon].AddVolatileStatus(StatusCaused());
        return base.Use(caster, foe);
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.Rage;
    }
}

public class HydroPump : Movements
{
    public HydroPump()
    {
        name = "Hydro Pump";
        attack = 110;
        heal = 0;
        PP = 5;
        type = ElementalTypes.Water;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Magic;
    }
}

public class Slash : Movements
{
    public Slash()
    {
        name = "Slash";
        attack = 70;
        heal = 0;
        PP = 15;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
}
public class Slam : Movements
{
    public Slam()
    {
        name = "Slam";
        attack = 80;
        heal = 0;
        PP = 20;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override bool MovementAccuracy(Trainer caster, Trainer foe)
    {
        Random random = new Random();
        int strikeChances = random.Next(1, 100);
        if (strikeChances < 76)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class Fissure : Movements
{
    public Fissure()
    {
        name = "Fissure";
        attack = 0;
        heal = 0;
        PP = 5;
        type = ElementalTypes.Normal;
        movementType = MovementType.OneHitKO;
        damageClass = DamageClass.Melee;
    }
    public override bool MovementAccuracy(Trainer caster, Trainer foe)
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 100);        
        if (randomNumber < 31)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}



public class HornDrill : Movements
{
    public HornDrill()
    {
        name = "Horn Drill";
        attack = 0;
        heal = 0;
        PP = 5;
        type = ElementalTypes.Normal;
        movementType = MovementType.OneHitKO;
        damageClass = DamageClass.Melee;
    }
    public override bool MovementAccuracy(Trainer caster, Trainer foe)
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 100);
        int accuracyChances = caster.pokemonList[caster.battlePokemon].GetLevel() - foe.pokemonList[foe.battlePokemon].GetLevel() + 30;
        if (randomNumber < accuracyChances)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class RockThrow : Movements
{
    public RockThrow()
    {
        name = "Rock Throw";
        attack = 50;
        heal = 0;
        PP = 15;
        type = ElementalTypes.Rock;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override bool MovementAccuracy(Trainer caster, Trainer foe)
    {
        Random random = new Random();
        int accuracyChances = random.Next(1, 100);
        if (accuracyChances < 66)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


public class HyperBeam : Movements, ICauseVStatus, ITurnLimitedEffect
{
    public HyperBeam()
    {
        name = "Hyper Beam";
        attack = 150;
        heal = 0;
        PP = 5;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Magic;
    }
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        movementPokemon.AddVolatileStatus(StatusCaused());
        base.Use(movementPokemon, foePokemon);
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public int SetStatusTurns()
    {
        Console.WriteLine("El pokemon está cargando su ataque");
        return 1;
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.Charge;
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
        if (foePokemon.GetNonVolatileStatus() == NonVolatileStatus.Burn)
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
        movementType = MovementType.AutoDamage;
        damageClass = DamageClass.Melee;
    }
    public override Pokemon Use(Pokemon pokemon)
    {
        //falta implementar el autodaño
        Console.WriteLine("El movimiento " + name + "fue usado contra sí mismo");
        return pokemon;
    }
}

public class MirrorMove : Movements
{
    public MirrorMove()
    {
        name = "Mirror Move";
        attack = 0;
        heal = 0;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        movementPokemon.movementChosen = foePokemon.movementChosen;
        Console.WriteLine("El pokemon ha imitado el movimiento " + name + "fue usado");
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
}

public class Reflect : Movements, ICauseVStatus
{
    public Reflect()
    {
        name = "Reflect";
        attack = 0;
        heal = 0;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + movementPokemon.movementChosen.name + "fue usado. Su defensa aumentó!");
        PP--;
        if ((movementPokemon.GetVolatileStatus().HasFlag(VolatileStatus.DefenseBoost)))
        {
            Console.WriteLine("El pokemon ya se encuentra afectado, nada sucedió.");
        }
        else
        {
            movementPokemon.AddVolatileStatus(VolatileStatus.DefenseBoost);
            movementPokemon.battleDefense += 2;
        }
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.DefenseBoost;
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
        return VolatileStatus.DefenseBoost;
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
        movementType = MovementType.AffectsTrainer;
        damageClass = DamageClass.Melee;
    }

    public override KeyValuePair<Trainer, Trainer> Use(Trainer caster, Trainer foe)
    {
        Random newRandomPokemon = new Random();
        List<int> alivePokemonsByIndex = foe.AlivePokemonsByIndex();
        alivePokemonsByIndex.Remove(foe.battlePokemon);
        foe.battlePokemon = alivePokemonsByIndex[newRandomPokemon.Next(alivePokemonsByIndex.Count)];
        return new KeyValuePair<Trainer, Trainer>(caster, foe);
    }
}

public class Harden : Movements, ICauseVStatus
{
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + movementPokemon.movementChosen.name + "fue usado. Su defensa aumentó!");
        PP--;
        if ((movementPokemon.GetVolatileStatus() | VolatileStatus.DefenseBoost) == VolatileStatus.DefenseBoost)
        {
            Console.WriteLine("El pokemon ya se encuentra afectado, nada sucedió.");
        }
        else
        {
            movementPokemon.AddVolatileStatus(VolatileStatus.DefenseBoost);
            movementPokemon.battleDefense += 2;
        }
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.DefenseBoost;
    }
}

public class IcePunch : Movements
{
    public IcePunch()
    {
        name = "Ice Punch";
        attack = 75;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Ice;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }


}

public class FuryAttack : Movements, IMultipleAttack
{
    public FuryAttack()
    {
        name = "Fury Attack";
        attack = 15;
        heal = 0;
        PP = 20;
        maxPP = 20;
        type = ElementalTypes.Normal;
        movementType = MovementType.MultipleAttack;
        damageClass = DamageClass.Melee;
    }
    public int AttackTimes()
    {
        Random random = new Random();
        int attackChances = random.Next(1, 100);
        if (attackChances < 37)
        {
            return 2;
        }
        if (attackChances > 37 && attackChances < 74)
        {
            return 3;
        }
        if (attackChances > 74 && attackChances < 87)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }
}

public class FireSpin : Movements, IMultipleAttack
{
    public FireSpin()
    {
        name = "Fire Spin";
        attack = 25;
        heal = 0;
        PP = 20;
        maxPP = 20;
        type = ElementalTypes.Fire;
        movementType = MovementType.MultipleAttack;
        damageClass = DamageClass.Magic;
    }
    public int AttackTimes()
    {
        Random random = new Random();
        int attackChances = random.Next(1, 100);
        if (attackChances < 37)
        {
            return 2;
        }
        if (attackChances > 37 && attackChances < 74)
        {
            return 3;
        }
        if (attackChances > 74 && attackChances < 87)
        {
            return 4;
        }
        else
        {
            return 5;
        }
    }
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
        Console.WriteLine("El movimiento " + name + " fue usado");
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
public class Roar : Movements
{
    public Roar()
    {
        name = "Roar";
        attack = 0;
        heal = 0;
        PP = 10;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
    public override void Use()
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
        Console.WriteLine("No sucedió nada!");
        PP--;
    }
}
public class Leer : Movements, ICauseVStatus, ITurnLimitedEffect
{
    public Leer()
    {
        name = "Leer";
        attack = 0;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
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
        return 5;
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.Leer;
    }
}

public class TailWhip : Movements, ICauseVStatus, ITurnLimitedEffect
{
    public TailWhip()
    {
        name = "Tail Whip";
        attack = 0;
        heal = 0;
        PP = 10;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
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
        return 5;
    }
    public VolatileStatus StatusCaused()
    {
        return VolatileStatus.TailWhip;
    }
}

public class FirePunch : Movements
{
    public FirePunch()
    {
        name = "Fire Punch";
        attack = 75;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Fire;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
}
public class FireBlast : Movements
{
    public FireBlast()
    {
        name = "Fire Blast";
        attack = 110;
        heal = 0;
        PP = 5;
        maxPP = 5;
        type = ElementalTypes.Fire;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Magic;
    }
}


public class FocusEnergy : Movements, ICauseNVStatus
{
    public FocusEnergy()
    {
        name = "Focus Energy";
        attack = 0;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }

    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        movementPokemon.SetNonVolatileStatus(StatusCaused());
        Console.WriteLine("El movimiento " + name + "fue usado, el ataque del pokemon subió para el resto de la pelea!");
        movementPokemon.battleAttack = Convert.ToInt32(movementPokemon.battleAttack * 1.5);
        PP--;
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }

    public NonVolatileStatus StatusCaused()
    {
        return NonVolatileStatus.Focus;
    }
}
public class HiJumpKick : Movements
{
    public HiJumpKick()
    {
        name = "Hi Jump Kick";
        attack = 110;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Fighting;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
}

public class MegaKick : Movements
{
    public MegaKick ()
    {
        name = "Mega Kick";
        attack = 120;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Fighting;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }

    public override bool MovementAccuracy(Trainer caster, Trainer foe)
    {
        Random random = new Random();
        int strikeChances = random.Next(1, 100);
        if (strikeChances < 76)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class ThunderPunch : Movements
{
    public ThunderPunch()
    {
        name = "Thunder Punch";
        attack = 75;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Electric;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
}
public class JumpKick : Movements
{
    public JumpKick()
    {
        name = "Jump Kick";
        attack = 100;
        heal = 0;
        PP = 10;
        maxPP = 10;
        type = ElementalTypes.Fighting;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }
}

public class Counter : Movements
{
    public Counter()
    {
        name = "Counter";
        attack = 0;
        heal = 0;
        PP = 15;
        maxPP = 15;
        type = ElementalTypes.Normal;
        movementType = MovementType.Counter;
        damageClass = DamageClass.Melee;
    }
}
public class Ember : Movements, ICauseNVStatus
{
    public Ember()
    {
        name = "Ember";
        attack = 40;
        heal = 0;
        PP = 25;
        type = ElementalTypes.Fire;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Magic;
    }
    public override KeyValuePair<Pokemon, Pokemon> Use(Pokemon movementPokemon, Pokemon foePokemon)
    {
        Console.WriteLine("El movimiento " + name + "fue usado");
        PP--;
        if (RandomBurn())
        {
            foePokemon.SetNonVolatileStatus(StatusCaused());
            Console.WriteLine("El pokemon " + foePokemon.name + "fue quemado!");
        }
        return new KeyValuePair<Pokemon, Pokemon>(movementPokemon, foePokemon);
    }
    public int SetStatusTurns()
    {
        return 1;
    }
    public NonVolatileStatus StatusCaused()
    {
        return NonVolatileStatus.Burn;
    }
    public bool RandomBurn()
    {
        Random randomBurn = new Random();
        if (randomBurn.Next(0, 9) == 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class TakeDown : Movements, IDamageBoth
{
    public TakeDown()
    {
        name = "Take Down";
        attack = 90;
        heal = 0;
        PP = 25;
        type = ElementalTypes.Normal;
        movementType = MovementType.Damage;
        damageClass = DamageClass.Melee;
    }      
    public int AutoDamage(int damage)
    {
        return Convert.ToInt32(damage * 0.25);
    }
}
public class Submission : Movements, IDamageBoth
{
    public Submission()
    {
        name = "Submission";
        attack = 80;
        heal = 0;
        PP = 20;
        type = ElementalTypes.Fighting;
        movementType = MovementType.DamageBoth;
        damageClass = DamageClass.Melee;
    }
    public int AutoDamage(int damage)
    {
        return Convert.ToInt32(damage * 0.25);
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
                case "Fire spin":
                    return new FireSpin();
                case "Reflect":
                    return new Reflect();
                case "Thunder Punch":
                    return new ThunderPunch();
                case "Ice Punch":
                    return new IcePunch();
                case "Fire Punch":
                    return new FirePunch();
                case "Roar":
                    return new Roar();
                case "Leer":
                    return new Leer();
                case "Ember":
                    return new Ember();
                case "Take Down":
                    return new TakeDown();
                case "Dragon Rage":
                    return new DragonRage();
                case "Hydro Pump":
                    return new HydroPump();
                case "Hyper Beam":
                    return new HyperBeam();
                case "Jump Kick":
                    return new JumpKick();
                case "Focus Energy":
                    return new FocusEnergy();
                case "Hi Jump Kick":
                    return new HiJumpKick();
                case "Mega Kick":
                    return new MegaKick();
                case "Mirror Move":
                    return new MirrorMove();
                case "Fire Blast":
                    return new FireBlast();
                case "Rage":
                    return new Rage();
                case "Slash":
                    return new Slash();
                case "Fire Spin":
                    return new FireSpin();
                case "Rock Throw":
                    return new RockThrow();
                case "Slam":
                    return new Slam();
                case "Harden":
                    return new Harden();
                case "Counter":
                    return new Counter();
                case "Fissure":
                    return new Fissure();
                case "Submission":
                    return new Submission();
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
