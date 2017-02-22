using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    class EnemyList : List<Enemy>
    {
        protected GameObject playerSelf;

        public EnemyList(GameObject playerSelf)
        {
            this.playerSelf = playerSelf;
        }

        /// <summary>
        /// Populate list with all enemy players
        /// </summary>
        public void PopulateList()
        {
            GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject enemyGameObject in enemyGameObjects)
            {
                if (enemyGameObject != playerSelf && enemyGameObject.activeSelf) {
                    Enemy enemy = new Enemy(enemyGameObject); 
                    this.Add(enemy);
                   
                }
            }
        }

        /// <summary>
        /// Populate list with all enemy players
        /// </summary>
        public void UpdateList()
        {
           /* foreach (Enemy enemy in this)
            {

            }*/
        }

        /// <summary>
        /// Populate list with all enemy players
        /// </summary>
        public Enemy GetEnemyWithHighestPriorityLevel()
        {
            Enemy enemyToReturn = null;
            foreach (Enemy enemy in this)
            {
                if(enemy.IsAlive() && (enemyToReturn == null || enemy.priorityLevelForPlayer > enemyToReturn.priorityLevelForPlayer ))
                {
                    enemyToReturn = enemy;
                }
            }
            return enemyToReturn;
        }
    }
}
