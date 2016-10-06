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

        public void PopulateList()
        {
            GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject enemyGameObject in enemyGameObjects)
            {
                if(enemyGameObject != playerSelf) {
                    Enemy enemy = new Enemy(enemyGameObject); 
                    this.Add(enemy);
                }
            }
        }
    }
}
