using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{
    /// <summary>
    /// Tests the AI can correctly prioritise enemies using factors determined in EnemySense
    /// </summary>
    class EnemySense_ShootingTest : MattBot
    {

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// </summary>
        protected override void UpdatePlayerState()
        {
            enemyList.UpdateList();

            // Check status on all enemies
            enemySense.Update();

            Enemy enemyTarget = enemyList.GetEnemyWithHighestPriorityLevel();
            if (enemyTarget != null)
            {
                if (enemyTarget.CanBeShot()) {
                    this.shootPrimaryWeapon = true;
                }
                this.rotatePlayer = enemyTarget.fastestWayForPlayerToRotateToEnemy;
                // Enemy enemyTarget = GetHighestPriorityEnemyTarget();
            }
        }
    }
}
