using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{
    /// <summary>
    /// Responsible for detecting where enemies are, whether they are in shooting range
    /// </summary>
    class EnemySense : Sense
    {
        protected Transform playerSelfTransform;
        protected Transform playerSelfGunTransform;
        protected EnemyList enemyList;
        protected MattBot playerSelfScript;
        protected float minShootingDistance;

        public EnemySense(Transform playerSelf, EnemyList enemyList)
        {
            this.enemyList = enemyList;
            this.playerSelfTransform = playerSelf;
            this.playerSelfGunTransform = playerSelf.FindChild("Gun");
            this.playerSelfScript = playerSelf.GetComponent<MattBot>();
            this.minShootingDistance = (PrimaryWeaponProjectile.projectileVelocity * PrimaryWeaponProjectile.timeToLive) + 1f;
        }

        public void Update()
        {
            foreach (Enemy enemy in enemyList)
            {
                // Careful with the ordering...
                RaycastHit hitInfo = new RaycastHit();
                enemy.isBehindCover = Sense.Linecast(playerSelfTransform.position, enemy.gameObject.transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Environment"), Color.gray);
                enemy.distanceFromPlayer = Vector3.Distance(playerSelfTransform.position, enemy.gameObject.transform.position);
                enemy.distanceFromPlayerGun = Vector3.Distance(playerSelfGunTransform.position, enemy.gameObject.transform.position);
                enemy.predictedMovementDirection = this.GetPredictedMovementDirection(enemy);
                enemy.predictedRotation = enemy.basePlayerScript.rotatePlayer;
                float timeItWouldTakePimaryWeaponProjectileToHitEnemy = enemy.distanceFromPlayerGun / (PrimaryWeaponProjectile.projectileVelocity * Time.fixedDeltaTime);
                enemy.predictedPosition = MovementPrediction.EstimatePositionAfterTime(enemy.lastPosition, enemy.gameObject.transform, BasePlayer.moveVelocity, timeItWouldTakePimaryWeaponProjectileToHitEnemy, enemy.lastRotation, enemy.predictedRotation, enemy.predictedMovementDirection);
                enemy.fastestWayForPlayerToRotateToEnemy = PlayerRotation.GetRotationTypeToFaceTarget(playerSelfGunTransform, enemy.predictedPosition);
                enemy.timeForPlayerToRotateToEnemy = PlayerRotation.GetTimeRequiredToFaceTarget(playerSelfGunTransform, enemy.predictedPosition);
                enemy.timeForEnemyToRotateToPlayer = PlayerRotation.GetTimeRequiredToFaceTarget(enemy.gameObject.transform, playerSelfTransform.gameObject.transform.position);
                enemy.lastPosition = new Vector3(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y, enemy.gameObject.transform.position.z);
                enemy.lastRotation = enemy.basePlayerScript.rotatePlayer;
                enemy.lastdMovementDirection = enemy.basePlayerScript.movePlayer;
                enemy.priorityLevelForPlayer = this.CalculateEnemyPriorityFactor(enemy);
                if (enemy.IsAlive()) { 
                    Debug.DrawLine(playerSelfGunTransform.position, enemy.predictedPosition, Color.green);
                }
            }
        }

        public float CalculateEnemyPriorityFactor(Enemy enemy)
        {
            if (enemy.IsAlive() == false)
            {
                return 0f;
            }
            float isOutOfShootingRangeFactor = (enemy.distanceFromPlayer < minShootingDistance ? 1 : 0) * 0.4f;
            // TODO maybe distanceToEnemy should be multiplied by time taken to turn and facing directions?
            float distanceToEnemyFactor = (1f - (enemy.distanceFromPlayer / 30f)) * 0.2f;
            float timeRequiredToRotate180Degrees = (Mathf.Deg2Rad * 180) / (BasePlayer.rotationVelocity * Time.fixedDeltaTime);
            float timeForEnemyToTurnToYouFactor = (1f - (enemy.timeForEnemyToRotateToPlayer / timeRequiredToRotate180Degrees)) * 0.14f;
            float isBehindCoverFactor = (enemy.isBehindCover ? 0 : 1) * 0.2f;
            float timeForSelfToTurnToEnemyFactor = (1f - (enemy.timeForPlayerToRotateToEnemy / timeRequiredToRotate180Degrees)) * 0.14f;
            // TODO amount of damage dealt to self * 0.04f
            // TODO amount of damage dealt to others * 0.02f
            float healthRemainingFactor = (1f - ((float)enemy.basePlayerScript.GetHealth() / 100f)) * 0.02f;
            float totalPriorityFactor = isOutOfShootingRangeFactor + distanceToEnemyFactor + timeForEnemyToTurnToYouFactor + isBehindCoverFactor + timeForSelfToTurnToEnemyFactor + healthRemainingFactor;
          //  Debug.Log("SHOOTABLE " + isOutOfShootingRangeFactor + " * DISTANCE " + distanceToEnemyFactor + " ENEMY TURN " + timeForEnemyToTurnToYouFactor + " PLAYER TURN " + timeForSelfToTurnToEnemyFactor + " NOT IN COVER " + isBehindCoverFactor + " HEALTH " + healthRemainingFactor + " = " + totalPriorityFactor);
            return totalPriorityFactor;
        }

        public BasePlayer.movementTypes GetPredictedMovementDirection(Enemy enemy)
        {
            if (enemy.basePlayerScript.movePlayer != BasePlayer.movementTypes.None)
            {
                return enemy.basePlayerScript.movePlayer;
            }
            else
            {
                return enemy.lastdMovementDirection;
            }
        }
    }
}
