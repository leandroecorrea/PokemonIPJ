using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class Liga
{
    int trainersDefeated;
    int totalEnemies;    
    EnemyTrainer enemyTrainer;    
    int healsRemaining;
    enum LigaOptions {Battle, Stats, Heal, Save, BackToMenu, Exit}
    bool gamePlay = true;
    Battle battle;
    string[] enemiesList;



    public Liga(Player player)
    {                
        healsRemaining = 2;
        trainersDefeated = 0;
        enemiesList = File.ReadAllLines("enemieslist.txt");
        totalEnemies = enemiesList.Length;             
    }   

    public bool Lobby(Player player)
    {
        Console.WriteLine("Elija su opción: 1 - Luchar 2- Stats 3 - Curar pokemones 4- Guardar 5- Volver al menu 6- Salir");
        LigaOptions option = (LigaOptions)Convert.ToInt32(Console.ReadLine());
        switch (option)
        {
            case LigaOptions.Battle:
                battle = new Battle();
                player = battle.LigaBattle(player, EnemyCreator());
                trainersDefeated++;
                break;
            case LigaOptions.Stats:
                break;
            case LigaOptions.Heal:
                break;
            case LigaOptions.Save:
                break;
            case LigaOptions.BackToMenu:
                gamePlay = false;
                Game.GetInstance().status = Game.Status.Menu;
                break;
            case LigaOptions.Exit:
                gamePlay = false;
                Game.GetInstance().status = Game.Status.Exit;
                break;
            default:
                break;
        }
        return gamePlay;
    }
    public EnemyTrainer EnemyCreator()
    {
        int currentEnemy = this.trainersDefeated;
        enemyTrainer = new EnemyTrainer(enemiesList[currentEnemy], enemiesList[currentEnemy] + "/" + enemiesList[currentEnemy] + ".txt");
        return enemyTrainer;
    }
}



