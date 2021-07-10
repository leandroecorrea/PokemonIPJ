using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class Battle
{

    enum STATUS { HEAL, RETREAT, ATTACK }
    STATUS status;
    int turnsParalyzed = 0;
    int turnsToBeParalyzed = 0;
    int turnsAsleep = 0;
    int turnsToSleep = 0;
    int turnsConfused = 0;
    int turnsToBeConfused = 0;
    int chargeTurns = 0;
    int turnsToCharge = 0;

    public Battle()
    {

    }
    public Player LigaBattle(Player player, EnemyTrainer currentEnemyTrainer)
    {
        player.battlePokemon = InitialBattlePokemon(player.pokemonList);
        currentEnemyTrainer.battlePokemon = InitialBattlePokemon(currentEnemyTrainer.pokemonList);
        bool battleLoop = true;
        while (battleLoop)
        {            
            (player, currentEnemyTrainer) = PlayerTurno(player, currentEnemyTrainer);
            battleLoop = BattleChecker(player);
            if (battleLoop)
            {
                (player, currentEnemyTrainer) = EnemyTurno(currentEnemyTrainer, player);
                battleLoop = BattleChecker(currentEnemyTrainer);
            }
        }
        return player;
    }
    public KeyValuePair<Player, EnemyTrainer> PlayerTurno(Player player, EnemyTrainer currentEnemyTrainer)
    {
        bool wrongOption;
        do
        {
            Console.WriteLine("1 - Movimiento 2 - Cambiar pokemon");
            int option = Convert.ToInt32(Console.ReadLine());
            switch (option)
            {
                case 1:
                    (player, currentEnemyTrainer) = PlayerMove(player, currentEnemyTrainer);
                    wrongOption = false;
                    break;
                case 2:
                    ChangePokemon(player);
                    wrongOption = false;
                    break;
                default:
                    wrongOption = true;
                    break;
            }
        } while (wrongOption);
        return new KeyValuePair<Player, EnemyTrainer>(player, currentEnemyTrainer);
    }
    public KeyValuePair<Player, EnemyTrainer> PlayerMove(Player player, EnemyTrainer currentEnemyTrainer)
    {
        (player.pokemonList[player.battlePokemon], currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon]) = CheckStatus(player.pokemonList[player.battlePokemon], currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon]);
        if (player.pokemonList[player.battlePokemon].canAttack)
        {
            if (player.pokemonList[player.battlePokemon].autoAttack)
            {
                player.pokemonList[player.battlePokemon].autoAttack = false;
            }
            else
            {
                player.pokemonList[player.battlePokemon] = PlayerSetMove(player.pokemonList[player.battlePokemon]);
            }
        }
        return BattleMovement(player, currentEnemyTrainer);
    }
    public Pokemon PlayerSetMove(Pokemon pokemon)
    {
        bool movement;
        int option;
        do
        {
            Console.WriteLine("Elige entre estos movimientos: ");
            //lista auxiliar para acceder a movimiento elegido por key

            for (int i = 0; i < pokemon.movementList.Count; i++)
            {
                Console.WriteLine(i + pokemon.movementList[i].name);
                //battleMoves.Add(movements.name);
            }
            option = Convert.ToInt32(Console.ReadLine());
            if (pokemon.movementList[option].PP == 0)
            {
                Console.WriteLine("No cuentas con mas PP para ese movimiento");
                movement = false;
            }
            else
            {
                movement = true;
            }

        } while (!movement);
        pokemon.movementChosen = pokemon.movementList[option];
        return pokemon;
    }
    public KeyValuePair<Player, EnemyTrainer> EnemyTurno(EnemyTrainer currentEnemyTrainer, Player player)
    {
        (currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon], player.pokemonList[player.battlePokemon]) = CheckStatus(currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon], player.pokemonList[player.battlePokemon]);
        if (currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].canAttack)
        {
            if (currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].autoAttack)
            {
                currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].autoAttack = false;
            }
            else
            {
                (player, currentEnemyTrainer) = AISwitcher(currentEnemyTrainer, player);
            }
        }        
        return BattleMovement(currentEnemyTrainer, player); 
    }
    public KeyValuePair<Player, EnemyTrainer> AISwitcher(EnemyTrainer currentEnemyTrainer, Player player)
    {
        status = SetEnemyStatus(currentEnemyTrainer);
        switch (status)
        {
            case STATUS.HEAL:
                currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].movementChosen = SetEnemyHealMove(currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon]);
                return BattleMovement(currentEnemyTrainer, player);                
            case STATUS.RETREAT:
                currentEnemyTrainer.battlePokemon = RetreatEnemyPokemon(currentEnemyTrainer.pokemonList);
                Console.WriteLine("El enemigo cambió su pokemon, " + currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].name + " fue elegido");
                return ReturnCaster(currentEnemyTrainer, player);                
            case STATUS.ATTACK:
                currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].movementChosen = SetEnemyDamageMove(currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon], player.pokemonList[player.battlePokemon]);
                return BattleMovement(currentEnemyTrainer, player);                
            default:
                return ReturnCaster(currentEnemyTrainer, player);                
        }
    }
    public KeyValuePair<Player, EnemyTrainer> BattleMovement(Trainer caster, Trainer foe)
    {
        (caster, foe) = UseMovement(caster, foe);
        return ApplyMovement(caster, foe);        
    }
    public KeyValuePair<Player, EnemyTrainer> ReturnCaster(Trainer caster, Trainer foe)
    {
        if (caster is EnemyTrainer)
        {
            return new KeyValuePair<Player, EnemyTrainer>(foe as Player, caster as EnemyTrainer);
        }
        else
        {
            return new KeyValuePair<Player, EnemyTrainer>(caster as Player, foe as EnemyTrainer);
        }
    }
    public KeyValuePair<Player, EnemyTrainer> UseMovement(Trainer caster, Trainer foe)
    {
        if (caster.pokemonList[caster.battlePokemon].movementChosen is INonTrainableMove)
        {
            caster.pokemonList[caster.battlePokemon].movementChosen.Use();
        }
        else
        {
            foreach (Movements movements in caster.pokemonList[caster.battlePokemon].movementList)
            {
                if (movements == caster.pokemonList[caster.battlePokemon].movementChosen)
                {
                    (caster.pokemonList[caster.battlePokemon], foe.pokemonList[caster.battlePokemon]) = movements.Use(caster.pokemonList[caster.battlePokemon], foe.pokemonList[caster.battlePokemon]);                    
                }
            }
        }
        return ReturnCaster(caster,foe);
    }
    public KeyValuePair<Player, EnemyTrainer> ApplyMovement(Trainer caster, Trainer foe)
    {
        switch (caster.pokemonList[caster.battlePokemon].movementChosen.movementType)
        {
            case (Movements.MovementType.Damage):
                foe.pokemonList[foe.battlePokemon].HP -= Damage(caster.pokemonList[caster.battlePokemon], foe.pokemonList[foe.battlePokemon]);
                break;
            case (Movements.MovementType.Heal):
                caster.pokemonList[caster.battlePokemon].HP += Heal(caster.pokemonList[caster.battlePokemon]);
                break;
            case (Movements.MovementType.Mix):
                break;
            default:
                break;
        }
        SetStatusDuration(caster.pokemonList[caster.battlePokemon].movementChosen);
        return ReturnCaster(caster, foe);
    }
    public void SetStatusDuration(Movements movementChosen)
    {
        if (movementChosen is ITurnLimitedEffect)
        {
            if (movementChosen is ICauseNVStatus)
            {
                switch (((ICauseNVStatus)movementChosen).StatusCaused())
                {
                    case NonVolatileStatus.Paralysis:
                        turnsToBeParalyzed = (((ITurnLimitedEffect)movementChosen).SetStatusTurns());
                        break;
                    case NonVolatileStatus.Sleep:
                        turnsToSleep = (((ITurnLimitedEffect)movementChosen).SetStatusTurns());
                        break;
                    default:
                        break;
                }
            }
            else if (movementChosen is ICauseVStatus)
            {
                VolatileStatus statusCaused = (((ICauseVStatus)movementChosen).StatusCaused());                
                if ((statusCaused | VolatileStatus.Confusion) == VolatileStatus.Confusion)
                {
                    turnsToBeConfused = (((ITurnLimitedEffect)movementChosen).SetStatusTurns());
                }
                if ((statusCaused | VolatileStatus.Charge) == VolatileStatus.Charge)
                {
                    chargeTurns = (((ITurnLimitedEffect)movementChosen).SetStatusTurns());
                }
            }
        }
    }    
    STATUS SetEnemyStatus(EnemyTrainer enemyTrainer)
    {
        if (enemyTrainer.pokemonList[enemyTrainer.battlePokemon].ShouldHeal())
        {
            if (enemyTrainer.pokemonList[enemyTrainer.battlePokemon].CanHeal())
            {
                return STATUS.HEAL;
            }
            else
            {
                if (MayRetreat(enemyTrainer))
                {
                    return STATUS.RETREAT;
                }
                else
                {
                    return STATUS.ATTACK;
                }
            }
        }
        else
        {
            return STATUS.ATTACK;
        }
    }
    public bool MayRetreat(EnemyTrainer enemyTrainer)
    {
        int totalWeakPokemons = 0;
        for (int i = 0; i < enemyTrainer.pokemonList.Count; i++)
        {
            if (enemyTrainer.pokemonList[i].ShouldHeal())
            {
                totalWeakPokemons++;
            }
        }
        if (totalWeakPokemons < 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int RetreatEnemyPokemon(List<Pokemon> enemyPokeList)
    {
        Pokemon hpPokemon = enemyPokeList.Aggregate((longest, next) => next.HP > longest.HP ? next : longest);
        return enemyPokeList.IndexOf(hpPokemon);
    }
    public int Damage(Pokemon attackPokemon, Pokemon defensePokemon)
    {
        float bonus = Bonus(attackPokemon);
        float effectiveness = SetEfectiveness(attackPokemon.movementChosen.type, defensePokemon.pokeType);
        Random random = new Random();
        float variation = random.Next(85, 100);
        float pokeDefense = PokeDefense(attackPokemon);
        float level = attackPokemon.GetLevel();
        //Damage = ((((2 * Level / 5 + 2) * AttackStat * AttackPower / DefenseStat) / 50) + 2) * STAB * Weakness/Resistance * RandomNumber / 100        
        float pokeAttack = PokeAttack(attackPokemon);
        float attackPower = attackPokemon.movementChosen.attack;
        int damage = Convert.ToInt32(((((2 * level / 5 + 2) * attackPower * pokeAttack / pokeDefense) / 50) + 2) * bonus * effectiveness * (variation / 100));
        //int damage = Convert.ToInt32(0.01 * bonus * effectiveness * variation * ((((0.2 * level + 1) * pokeAttack * attackPower) / (25 * pokeDefense))) + 2);
        return damage;
    }
    public int SimpleDamage(Pokemon attackPokemon, Movements currentMovement, Dictionary<string, ElementalTypes> defensePokemonType)
    {
        float bonus = Bonus(attackPokemon, currentMovement.type);
        float effectiveness = SetEfectiveness(currentMovement.type, defensePokemonType);
        int damage = (((2 * attackPokemon.GetLevel() / 5 + 2) * currentMovement.attack / 50) + 2);
        damage = damage / 5 + 2;
        damage = Convert.ToInt32((float)damage * bonus * effectiveness);
        return damage;
    }
    public int Heal(Pokemon healPokemon)
    {

        int heal = healPokemon.movementChosen.heal;
        return heal;
    }
    public int InitialBattlePokemon(List<Pokemon> pokemonList)
    {
        int battlePokemon = 0;
        for (int i = 0; i < pokemonList.Count; i++)
        {
            if (pokemonList[i].HP < 0)
            {
                battlePokemon++;
            }
            else
            {
                return battlePokemon;
            }
        }
        return battlePokemon;
    }
    public void ChangePokemon(Player player)
    {
        Console.WriteLine("Elige qué pokemon querés que entre");
        for (int i = 0; i < player.pokemonList.Count; i++)
        {
            Console.WriteLine(i + " - " + player.pokemonList[i]);
        }
        player.battlePokemon = Convert.ToInt32(Console.ReadLine());
    }
    public float Bonus(Pokemon attackPokemon)
    {
        float bonus = 1f;
        foreach (ElementalTypes elementalTypes in attackPokemon.pokeType.Values)
        {
            if (attackPokemon.movementChosen.type == elementalTypes)
            {
                return 1.5f;
            }
        }
        return bonus;
    }   
    public float Bonus(Pokemon attackPokemon, ElementalTypes movementElementalType)
    {
        float bonus = 1f;
        foreach (ElementalTypes elementalTypes in attackPokemon.pokeType.Values)
        {
            if (attackPokemon.movementChosen.type == movementElementalType)
            {
                return 1.5f;
            }
        }
        return bonus;
    }   
    public float SetEfectiveness(ElementalTypes movementType, Dictionary<string, ElementalTypes> defensePokemonType)
    {
        float zeroEffectiveness = 0;
        float lowEffectiveness = 0.5f;
        float normalEffectiveness = 1;
        float highEffectiveness = 2;

        switch (movementType)
        {
            case ElementalTypes.Normal:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Rock)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Ghost)
                    {
                        return zeroEffectiveness;
                    }
                }
                break;

            case ElementalTypes.Fighting:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Flying || defenseType == ElementalTypes.Poison || defenseType == ElementalTypes.Bug || defenseType == ElementalTypes.Psychic)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Normal || defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Ice)
                    {
                        return highEffectiveness;
                    }
                }
                break;

            case ElementalTypes.Flying:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Electric)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Fighting || defenseType == ElementalTypes.Bug || defenseType == ElementalTypes.Grass)
                    {
                        return highEffectiveness;
                    }
                }
                break;

            case ElementalTypes.Poison:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Poison || defenseType == ElementalTypes.Ground || defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Ghost)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Grass)
                    {
                        return highEffectiveness;
                    }
                }
                break;

            case ElementalTypes.Ground:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Bug || defenseType == ElementalTypes.Grass)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Poison || defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Fire || defenseType == ElementalTypes.Electric)
                    {
                        return highEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Flying)
                    {
                        return zeroEffectiveness;
                    }
                }
                break;

            case ElementalTypes.Rock:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Fighting || defenseType == ElementalTypes.Ground)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Flying || defenseType == ElementalTypes.Bug || defenseType == ElementalTypes.Fire || defenseType == ElementalTypes.Ice)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Bug:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Fighting || defenseType == ElementalTypes.Flying || defenseType == ElementalTypes.Poison || defenseType == ElementalTypes.Ghost || defenseType == ElementalTypes.Fire)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Grass || defenseType == ElementalTypes.Psychic)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Ghost:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Normal)
                    {
                        return zeroEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Ghost || defenseType == ElementalTypes.Psychic)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Fire:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Fire || defenseType == ElementalTypes.Water || defenseType == ElementalTypes.Dragon)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Bug || defenseType == ElementalTypes.Grass || defenseType == ElementalTypes.Ice)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Water:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Water || defenseType == ElementalTypes.Grass || defenseType == ElementalTypes.Dragon)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Ground || defenseType == ElementalTypes.Fire)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Grass:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Fighting || defenseType == ElementalTypes.Poison || defenseType == ElementalTypes.Bug || defenseType == ElementalTypes.Fire || defenseType == ElementalTypes.Grass || defenseType == ElementalTypes.Dragon)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Rock || defenseType == ElementalTypes.Ground || defenseType == ElementalTypes.Water)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Electric:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Ground)
                    {
                        return zeroEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Grass || defenseType == ElementalTypes.Electric || defenseType == ElementalTypes.Dragon)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Flying || defenseType == ElementalTypes.Water)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Psychic:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Psychic)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Fighting || defenseType == ElementalTypes.Poison)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Ice:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Fire || defenseType == ElementalTypes.Water)
                    {
                        return lowEffectiveness;
                    }
                    if (defenseType == ElementalTypes.Flying || defenseType == ElementalTypes.Ground || defenseType == ElementalTypes.Dragon)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            case ElementalTypes.Dragon:
                foreach (ElementalTypes defenseType in defensePokemonType.Values)
                {
                    if (defenseType == ElementalTypes.Dragon)
                    {
                        return highEffectiveness;
                    }
                }
                break;
            default:
                break;
        }
        return normalEffectiveness;
    }
    public Movements SetEnemyDamageMove(Pokemon attackPokemon, Pokemon defensePokemon)
    {
        //si un solo movimiento tiene PP struggle va a ser false
        bool struggle = true;
        List<Movements> enemyDamageMove = new List<Movements>();
        foreach (Movements movements in attackPokemon.movementList)
        {
            if (movements.PP > 0 && movements.movementType == Movements.MovementType.Damage)
            {                
                struggle = false;
            }
        }
        if (struggle)
        {
            return new Struggle();
        }
        else
        {
            foreach (Movements movements in attackPokemon.movementList)
            {
                if (movements.movementType == Movements.MovementType.Damage)
                {
                    enemyDamageMove.Add(movements);
                }
            }
        }
        int auxDamage = 0;
        int maxDamageInList = 0;
        for (int i = 0; i < enemyDamageMove.Count; i++)
        {
            if (i == 0)
            {
                attackPokemon.movementChosen = enemyDamageMove[i];
                maxDamageInList = SimpleDamage(attackPokemon, attackPokemon.movementList[i], defensePokemon.pokeType);                
            }
            else
            {
                auxDamage = SimpleDamage(attackPokemon, attackPokemon.movementList[i], defensePokemon.pokeType);
                if (auxDamage > maxDamageInList)
                { 
                    attackPokemon.movementChosen = enemyDamageMove[i];
                    maxDamageInList = auxDamage;
                }
            }
        }
        return attackPokemon.movementChosen;
    }
    public Movements SetEnemyHealMove(Pokemon attackPokemon)
    {
        List<Movements> enemyHealMove = new List<Movements>();
        foreach (Movements movement in attackPokemon.movementList)
        {
            if (movement.movementType == Movements.MovementType.Heal)
            {
                enemyHealMove.Add(movement);
            }
        }
        for (int i = 0; i < enemyHealMove.Count; i++)
        {
            if (i == 0)
            {
                attackPokemon.movementChosen = enemyHealMove[i];

            }
            if (enemyHealMove[i].heal > attackPokemon.movementChosen.heal)
            {
                attackPokemon.movementChosen = enemyHealMove[i];
            }
        }
        return attackPokemon.movementChosen;
    }
    public float PokeAttack(Pokemon attackPokemon)
    {
        float pokeAttack;
        if (attackPokemon.movementChosen.damageClass == Movements.DamageClass.Magic)
        {
            pokeAttack = attackPokemon.GetSpecialAttack();
        }
        else
        {
            pokeAttack = attackPokemon.GetAttack();
        }
        return pokeAttack;
    }
    public float PokeDefense(Pokemon attackPokemon)
    {
        float pokeDefense;
        if (attackPokemon.movementChosen.damageClass == Movements.DamageClass.Magic)
        {
            pokeDefense = attackPokemon.GetSpecialDefense();
        }
        else
        {
            pokeDefense = attackPokemon.GetDefense();
        }
        return pokeDefense;
    }
    public bool BattleChecker(Player player)
    {
        bool battleLoop = true;
        if (player.pokemonList[player.battlePokemon].HP < 1)
        {
            player.remainingPokemons--;
            if (player.remainingPokemons == 0)
            {
                Game.GetInstance().status = Game.Status.Menu;
                Console.WriteLine("Derrotado...");
                battleLoop = false;
            }
            else
            {
                Console.WriteLine("Ha muerto tu pokemon! Elige qué Pokemon quieres que siga");
                for (int i = 0; i < player.pokemonList.Count; i++)
                {
                    if (player.pokemonList[i].HP < 1)
                    {
                        Console.WriteLine(player.pokemonList[i]);
                    }
                }
            }
        }
        return battleLoop;
    }
    public bool BattleChecker(EnemyTrainer enemyTrainer)
    {
        bool battleLoop = true;
        if (enemyTrainer.pokemonList[enemyTrainer.battlePokemon].HP < 1)
        {
            enemyTrainer.remainingPokemons--;
            Console.WriteLine(enemyTrainer.pokemonList[enemyTrainer.battlePokemon].name + "del enemigo fue derrotado");
            if (enemyTrainer.remainingPokemons == 0)
            {
                Console.WriteLine("Enemigo " + enemyTrainer.name + " fue derrotado!");
                battleLoop = false;
            }
            else
            {
                enemyTrainer.battlePokemon = InitialBattlePokemon(enemyTrainer.pokemonList);
                Console.WriteLine("El enemigo ha elegido a " + enemyTrainer.pokemonList[enemyTrainer.battlePokemon]);
            }
        }
        return battleLoop;
    }
    public KeyValuePair<Pokemon, Pokemon> CheckStatus(Pokemon pokemon, Pokemon enemyPokemon)
    {
        pokemon = CheckNonVolatileStatus(pokemon);
        (pokemon, enemyPokemon) = CheckVolatileStatus(pokemon, enemyPokemon);
        return new KeyValuePair<Pokemon, Pokemon>(pokemon, enemyPokemon);
    }
    public Pokemon CheckNonVolatileStatus(Pokemon pokemon)
    {
        switch (pokemon.GetNonVolatileStatus())
        {
            case NonVolatileStatus.Burn:
                pokemon = Burn(pokemon);
                break;
            case NonVolatileStatus.Freeze:
                pokemon.canAttack = Freeze(pokemon.name);
                break;
            case NonVolatileStatus.Paralysis:
                pokemon = Paralysis(pokemon);
                turnsParalyzed++;
                if (turnsParalyzed == 3)
                {
                    Console.WriteLine("El pokemon " + pokemon.name + " fue liberado de la parálisis");
                    pokemon.SetNonVolatileStatus(NonVolatileStatus.None);
                    turnsParalyzed = 0;
                }
                break;
            case NonVolatileStatus.Poison:
                pokemon.HP -= Poison(pokemon.GetMaxHP());
                break;
            case NonVolatileStatus.BadPoison:
                break;
            case NonVolatileStatus.Sleep:
                pokemon = Sleep(pokemon);
                break;
            default:
                break;
        }
        return pokemon;
    }
    public Pokemon Burn(Pokemon pokemon)
    {
        pokemon.battleAttack = pokemon.GetAttack() / 2;
        pokemon.HP -= Convert.ToInt32(pokemon.GetMaxHP() * 0.125);
        return pokemon;
    }
    public bool Freeze(string pokemonName)
    {
        Console.WriteLine("El pokemon " + pokemonName + " fue congelado!");
        return false;
    }
    public Pokemon Paralysis(Pokemon pokemon)
    {
        if (turnsParalyzed == turnsToBeParalyzed)
        {
            Console.WriteLine("El pokemon " + pokemon.name + " fue liberado de la parálisis");
            pokemon.SetNonVolatileStatus(NonVolatileStatus.None);
            pokemon.canAttack = true;
            turnsParalyzed = 0;
            turnsToBeParalyzed = 0;
        }
        else
        {
            Random random = new Random();
            int paralysisChance = random.Next(99);
            if (paralysisChance < 25)
            {
                Console.WriteLine("El pokemon está paralizado y no puede atacar!");
                pokemon.canAttack = false;
            }
            else
            {
                pokemon.canAttack = true;
            }
            turnsParalyzed++;
        }
        return pokemon;
    }
    public int Poison(int maxHP)
    {
        Console.WriteLine("El pokemon está envenenado y pierde HP");
        return Convert.ToInt32(maxHP / 16);
    }
    public Pokemon Sleep(Pokemon pokemon)
    {
        //esto ahora se va a fijar a partir del SetStatusDuration en el ApplyMovement
        //if (turnsAsleep == 0)
        //{
        //    Random random = new Random();
        //    turnsToSleep = random.Next(1, 7);
        //}        
        if (turnsAsleep == turnsToSleep)
        {
            Console.WriteLine("El pokemon " + pokemon.name + " se despertó!");
            pokemon.SetNonVolatileStatus(NonVolatileStatus.None);
            pokemon.canAttack = true;
            turnsAsleep = 0;
            turnsToSleep = 0;
        }
        else
        {
            Console.WriteLine("El pokemon está dormido y no puede atacar!");
            turnsAsleep++;
            pokemon.canAttack = false;
        }
        return pokemon;
    }
    public KeyValuePair<Pokemon,Pokemon> CheckVolatileStatus(Pokemon pokemon, Pokemon foePokemon)
    {
        // acá intenté usar los bitwise operators, me falta ver cómo hacer para que confusion no anule el canAttack que imponen los
        // status no volátiles
        if ((pokemon.GetVolatileStatus() | VolatileStatus.Confusion) == VolatileStatus.Confusion)
        {
            (pokemon, foePokemon) = Confusion(pokemon, foePokemon);
        }
        if ((pokemon.GetVolatileStatus() | VolatileStatus.Curse) == VolatileStatus.Curse)
        {
            pokemon.HP -= Curse(pokemon.GetMaxHP());
        }
        if ((pokemon.GetVolatileStatus() | VolatileStatus.Charge) == VolatileStatus.Charge)
        {
            pokemon = Charge(pokemon);
        }
        return new KeyValuePair<Pokemon, Pokemon> (pokemon, foePokemon);
    }
    public KeyValuePair<Pokemon,Pokemon> Confusion (Pokemon pokemon, Pokemon foePokemon)
    {
        // esto tmb va a ir en el SetStatusDuration()
        //turnsToBeConfused = TurnSetter(turnsConfused, turnsToBeConfused, pokemon.movementChosen);        
        if (turnsConfused == turnsToBeConfused)
        {
            Console.WriteLine("El pokemon " + pokemon.name + " se despabiló!");
            pokemon.SetNonVolatileStatus(NonVolatileStatus.None);
            pokemon.canAttack = true;
            turnsConfused = 0;
            turnsToBeConfused = 0;
        }
        else
        {
            Random confuseResult = new Random();
            int chances = confuseResult.Next(0, 99);
            if(chances < 50)
            {
                pokemon.canAttack = true;
                pokemon.autoAttack = true;
                pokemon.movementChosen = new Confused();
                //pokemon.movementChosen.Use(pokemon);
                //pokemon.HP -= Damage(pokemon, pokemon);
                //pokemon.canAttack = false;
            }
            if(chances > 50 && chances < 75)
            {
                Random randomMove = new Random();                
                Console.WriteLine("El pokemon está confundido!");
                pokemon.autoAttack = true;
                pokemon.movementChosen = pokemon.movementList[randomMove.Next(0,3)];
                turnsConfused++;
                pokemon.canAttack = true;
            }
            else
            {
                pokemon.canAttack = true;
            }            
        }
        return new KeyValuePair<Pokemon, Pokemon>(pokemon, foePokemon);
    }
    public int Curse(int maxHP)
    {
        return Convert.ToInt32(maxHP / 4);
    }
    public Pokemon Charge(Pokemon pokemon)
    {
        turnsToCharge = TurnSetter(chargeTurns, turnsToCharge, pokemon.movementChosen);
        if (turnsToCharge == chargeTurns)
        {
            Console.WriteLine("El pokemon " + pokemon.name + " se despabiló!");
            pokemon.SetNonVolatileStatus(NonVolatileStatus.None);
            pokemon.canAttack = true;
            turnsConfused = 0;
            turnsToBeConfused = 0;
        }
        else
        {
            Random confuseResult = new Random();
            int chances = confuseResult.Next(0, 99);
            if (chances < 50)
            {
                pokemon.movementChosen = new Confused();
                pokemon.movementChosen.Use(pokemon);
                pokemon.HP -= Damage(pokemon, pokemon);
                pokemon.canAttack = false;
            }
            if (chances > 50 && chances < 75)
            {
                Console.WriteLine("El pokemon está confundido!");
                pokemon.movementChosen = pokemon.movementList[1];
                turnsConfused++;
                pokemon.canAttack = false;
            }
            else
            {
                pokemon.canAttack = true;
            }
        }
        return pokemon;
    }
    public int TurnSetter(int turnsInStatus, int totalTurnsToBe, Movements movementChosen)
    {
        if (turnsInStatus == 0 && movementChosen is ITurnLimitedEffect)
        {
            return (((ITurnLimitedEffect)movementChosen).SetStatusTurns());
        }
        else
        {
            return totalTurnsToBe;
        }
    }
//  machete de docs de c# de microsoft
//   var flags = MyFlags.Pepsi | MyFlags.Coke;
//    var colas = MyFlags.Pepsi | MyFlags.Coke;
//    Console.WriteLine("Hay colas en flags {0}", (flags & colas) == colas ? "Yes" : "No"); //output Yes
//    flags &= ~MyFlags.Coke;
//    Console.WriteLine("Hay una cola en flags post cambio {0}", (flags | colas) == colas ? "Yes" : "No");//output yES
//}

}