using UnityEngine;
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
        private BulletList bulletList;
        private MattBot selfPlayerScript;

        public SituationManager(MattBot selfPlayerScript, EnemyList enemyList, BulletList bulletList)
        {
            this.enemyList = enemyList;
            this.bulletList = bulletList;
            this.selfPlayerScript = selfPlayerScript;
        }

        public void Update()
        {
            Enemy enemyTarget = enemyList.GetEnemyWithHighestPriorityLevel();
            if (enemyTarget != null) {
                selfPlayerScript.rotatePlayer = enemyTarget.fastestWayForPlayerToRotateToEnemy;
                if (enemyTarget.IsInRange())
                {
                    // TODO should only shoot if predicted position is exposed
                    selfPlayerScript.shootPrimaryWeapon = true;
                   
                }
                else
                {
                    selfPlayerScript.shootPrimaryWeapon = false;
                }
            }
            Bullet closestBullet = bulletList.GetClosestBulletToStrikingPlayerSelf();
            if (closestBullet != null && closestBullet.distanceFromStrikingPlayer < 4f)
            {
                //Debug.Log("DODGING");
                DodgeSense dodgeSense = new DodgeSense(selfPlayerScript, bulletList);
                selfPlayerScript.movePlayer = dodgeSense.DetermineBestDirectionForPlayerSelfToMove();
                //selfPlayerScript.movePlayer = BasePlayer.movementTypes.Left;
             //   selfPlayerScript.shootPrimaryWeapon = true;
            }
            else
            {
                selfPlayerScript.movePlayer = BasePlayer.movementTypes.None;
            }
        }
    }
}
