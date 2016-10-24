using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{

    class MattBot : BasePlayer
    {
        protected EnemyList enemyList;
        protected BulletList bulletList;
        protected EnemySense enemySense;
        protected BulletSense bulletSense;
        protected SituationManager situationManager;
        public const float playerRadius = 1.1f;

        /// <summary>
        /// Called once after bot is spawned. This is for intialising your bot code.
        /// </summary>
        protected override void InitPlayer()
        {
            enemyList = new EnemyList(gameObject);
            enemyList.PopulateList();
            bulletList = new BulletList(gameObject);
            enemySense = new EnemySense(this.transform, enemyList);
            bulletSense = new BulletSense(this, bulletList);
            situationManager = new SituationManager(this, enemyList, bulletList);
        }

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// </summary>
        protected override void UpdatePlayerState()
        {
            enemyList.UpdateList();
            bulletList.UpdateList();

            // Check status on all enemies
            enemySense.Update();
            bulletSense.Update();

            // Check status on all projectiles
            // TODO

            // Determine situation and best course of action
            situationManager.Update();
        }
    }
}
