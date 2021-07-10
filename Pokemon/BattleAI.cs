//using System;
//using System.Collections.Generic;
//using System.Text;

//class BattleAI
//{
//    enum STATUS { HEAL, RETREAT, ATTACK }
//    STATUS status;

//    public KeyValuePair<Player, EnemyTrainer> EnemyTurno(EnemyTrainer currentEnemyTrainer, Player player)
//    {
//        status = SetEnemyStatus(currentEnemyTrainer);
//        switch (status)
//        {
//            case STATUS.HEAL:
//                currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].movementChosen = SetEnemyHealMove(currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon]);
//                return UseMovement(currentEnemyTrainer, player);
//            case STATUS.RETREAT:
//                currentEnemyTrainer.battlePokemon = RetreatEnemyPokemon(currentEnemyTrainer.pokemonList);
//                break;
//            case STATUS.ATTACK:
//                currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon].movementChosen = SetEnemyDamageMove(currentEnemyTrainer.pokemonList[currentEnemyTrainer.battlePokemon], player.pokemonList[player.battlePokemon]);
//                return UseMovement(currentEnemyTrainer, player);
//            default:
//                break;
//        }
//        return new KeyValuePair<Player, EnemyTrainer>(player, currentEnemyTrainer);
//    }
//    STATUS SetEnemyStatus(EnemyTrainer enemyTrainer)
//    {
//        if (enemyTrainer.pokemonList[enemyTrainer.battlePokemon].ShouldHeal())
//        {
//            if (enemyTrainer.pokemonList[enemyTrainer.battlePokemon].CanHeal())
//            {
//                return STATUS.HEAL;
//            }
//            else
//            {
//                if (MayRetreat(enemyTrainer))
//                {
//                    return STATUS.RETREAT;
//                }
//                else
//                {
//                    return STATUS.ATTACK;
//                }
//            }
//        }
//        else
//        {
//            return STATUS.ATTACK;
//        }
//    }
//    public bool MayRetreat(EnemyTrainer enemyTrainer)
//    {
//        int totalWeakPokemons = 0;
//        for (int i = 0; i < enemyTrainer.pokemonList.Count; i++)
//        {
//            if (enemyTrainer.pokemonList[i].ShouldHeal())
//            {
//                totalWeakPokemons++;
//            }
//        }
//        if (totalWeakPokemons < 4)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    public int RetreatEnemyPokemon(List<Pokemon> enemyPokeList)
//    {
//        Pokemon hpPokemon = enemyPokeList.Aggregate((longest, next) => next.HP > longest.HP ? next : longest);
//        return enemyPokeList.IndexOf(hpPokemon);
//    }

//}

