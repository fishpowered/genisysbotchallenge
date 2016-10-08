using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{

    class MattBot : BasePlayer
    {
        public EnemyList enemyList;

        /// <summary>
        /// Called once after bot is spawned. This is for intialising your bot code.
        /// </summary>
        protected override void InitPlayer()
        {
            enemyList = new EnemyList(gameObject);
            enemyList.PopulateList();
            
            
        }

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// </summary>
        protected override void UpdatePlayerState()
        {
            enemyList.UpdateList();

            EnemySense enemySense = new EnemySense(this.transform, enemyList);
            enemySense.Check();
        }
    }
}
