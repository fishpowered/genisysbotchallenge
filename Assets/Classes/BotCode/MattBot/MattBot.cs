using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{

    public class MattBot : BasePlayer
    {
        public EnemyList enemyList;

        /// <summary>
        /// Called once after bot is spawned. This is for intialising your bot code.
        /// </summary>
        protected override void InitPlayer()
        {
            enemyList.PopulateList();
            
            
        }

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// Should always call UpdatePlayerXxx methods but the order these occur in is up to you.
        /// </summary>
        protected override void UpdatePlayerState()
        {

            EnemySense enemySense = new EnemySense(this.transform, enemyList);
            enemySense.check();
        }
    }
}
