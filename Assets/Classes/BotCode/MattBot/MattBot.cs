using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{

    class MattBot : BasePlayer
    {
        protected EnemyList enemyList;
        protected EnemySense enemySense;
        protected SituationManager situationManager;

        /// <summary>
        /// Called once after bot is spawned. This is for intialising your bot code.
        /// </summary>
        protected override void InitPlayer()
        {
            enemyList = new EnemyList(gameObject);
            enemyList.PopulateList();
            enemySense = new EnemySense(this.transform, enemyList);
            situationManager = new SituationManager(this, enemyList);
        }

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// </summary>
        protected override void UpdatePlayerState()
        {
            enemyList.UpdateList();

            // Check status on all enemies
            enemySense.Update();

            // Check status on all projectiles
            // TODO

            // Determine situation and best course of action
            situationManager.Update();
        }
    }
}
