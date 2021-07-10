using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class EnemyTrainer : Trainer
{
    //public string[] enemiesList = File.ReadAllLines("enemieslist.txt");    

    public EnemyTrainer()
    {
    }

    public EnemyTrainer(string name, string path) : base(name, path)
    {
            
    }

    //public List<string> EnemyReader(int currentEnemy)
    //{
    //    int nextEnemy = currentEnemy++;
        
    //}

}

