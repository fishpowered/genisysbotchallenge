using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    /// <summary>
    /// Given the state of various senses, determines what situation the bot is in and chooses the best course of action
    /// </summary>
    class SituationManager
    {
        private EnemyList enemyList;
        private MattBot selfPlayerScript;

        public SituationManager(MattBot selfPlayerScript, EnemyList enemyList)
        {
            this.enemyList = enemyList;
            this.selfPlayerScript = selfPlayerScript;
        }

        public void Update()
        {
            Enemy enemyTarget = enemyList.GetEnemyWithHighestPriorityLevel();
            if (enemyTarget != null) { 
                selfPlayerScript.rotatePlayer = enemyTarget.fastestWayForPlayerToRotateToEnemy;
                // Enemy enemyTarget = GetHighestPriorityEnemyTarget();
            }
        }
    }
}
