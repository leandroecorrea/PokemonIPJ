using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class Pokemon
{
    public string name;
    private int level;
    private int maxHP;
    public int HP;    
    private int attack;
    private int defense;
    private int specialAttack;
    private int specialDefense;
    public int battleAttack;
    public int battleDefense;
    public int battleSpecialAttack;
    public int battleSpecialDefense;
    public int turnsParalyzed = 0;
    public int turnsToBeParalyzed = 0;
    public int turnsAsleep = 0;
    public int turnsToSleep = 0;
    public int turnsConfused = 0;
    public int turnsToBeConfused = 0;
    public int chargeTurns = 0;
    public int turnsToCharge = 0;
    public int leerTurns = 0;
    public int turnsToBeLeered = 0;
    public int tailWhipTurns = 0;
    public int turnsToBeTailWhipped = 0;
    public int rageBoost = 0;
    public bool canAttack;
    public bool autoAttack;
    public Dictionary<string, ElementalTypes> pokeType = new Dictionary<string, ElementalTypes>();
    public List<Movements> movementList = new List<Movements>();
    public Movements movementChosen = new Movements();    
    NonVolatileStatus nonVolatileStatus = new NonVolatileStatus();
    VolatileStatus volatileStatus = new VolatileStatus();

    public Pokemon(string filePath)
    {
        ReadPokemon(filePath);
        maxHP = HP;
        battleAttack = attack;
        battleDefense = defense;
        battleSpecialAttack = specialAttack;
        battleSpecialDefense = specialDefense;
        nonVolatileStatus = NonVolatileStatus.None;
        volatileStatus = VolatileStatus.None;
        canAttack = true;
    }

    public void ReadPokemon(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);        
        name = lines[0];
        level = Convert.ToInt32(lines[1]);
        HP = Convert.ToInt32(lines[2]);
        attack = Convert.ToInt32(lines[3]);
        defense = Convert.ToInt32(lines[4]);
        specialAttack = Convert.ToInt32(lines[5]);
        specialDefense = Convert.ToInt32(lines[6]);
        string[] typesList = lines[7].Split(" ");
        for (int i = 0; i < typesList.Length; i++)
        {
            pokeType.Add(typesList[i], Types.GetInstance().GetType(typesList[i]));
        }
        movementList.Add(MovementDatabase.GetMovement(lines[8]));
        movementList.Add(MovementDatabase.GetMovement(lines[9]));
        movementList.Add(MovementDatabase.GetMovement(lines[10]));
        movementList.Add(MovementDatabase.GetMovement(lines[11]));        
    }  
    
    public int GetLevel()
    {
        return level;
    }
    public int GetMaxHP()
    {
        return maxHP;
    }
    public int GetAttack()
    {
        return attack;
    }
    public int GetDefense()
    {
        return defense;
    }
    public int GetSpecialAttack()
    {
        return specialAttack;
    }
    public int GetSpecialDefense()
    {
        return specialDefense;
    }
    public NonVolatileStatus GetNonVolatileStatus()
    {
        return nonVolatileStatus;
    }
    public void SetNonVolatileStatus(NonVolatileStatus nonVolatileStatus)
    {
        this.nonVolatileStatus = nonVolatileStatus;
    }
    public VolatileStatus GetVolatileStatus()
    {
        return volatileStatus;
    }
    public void AddVolatileStatus(VolatileStatus statusToAdd)
    {
        volatileStatus |= statusToAdd;
    }
    public void RemoveVolatileStatus(VolatileStatus statusToRemove)
    {
        try
        {
            volatileStatus &= ~statusToRemove;
        }
        catch
        {
            Console.WriteLine("VolatileStatus remotion error");
        }
    }
    public bool ShouldHeal()
    {
        if (HP < (maxHP * 0.40))
        {
            return true;
        }
        return false;
    }
    public bool MustRetreat()
    {
        if (HP < (maxHP * 0.1) && HP > (maxHP * 0.1))
        {
            return true;
        }
        return false;
    }
    public bool CanHeal()
    {
        bool canHeal = false;
        foreach (Movements movements in movementList)
        {
            if (movements.movementType == Movements.MovementType.Heal && movements.PP > 0)
            {
                canHeal = true;
            }
        }
        return canHeal;
    }
}

