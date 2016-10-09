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
        protected Transform self;
        protected Transform selfGun;
        protected EnemyList enemyList;
        public bool enemyInCover;
        protected MattBot selfPlayerScript;

        public EnemySense(Transform self, EnemyList enemyList)
        {
            this.enemyList = enemyList;
            this.self = self;
            this.selfGun = self.FindChild("Gun");
            this.selfPlayerScript = self.GetComponent<MattBot>();
        }

        public void Update()
        {
            foreach (Enemy enemy in enemyList)
            {
                // Careful with the ordering...
                RaycastHit hitInfo = new RaycastHit();
                enemy.isBehindCover = Sense.Linecast(self.position, enemy.gameObject.transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Environment"), Color.red);
                enemy.distanceFromPlayer = Vector3.Distance(self.position, enemy.gameObject.transform.position);
                enemy.distanceFromPlayerGun = Vector3.Distance(selfGun.position, enemy.gameObject.transform.position);
                float timeItWouldTakePimaryWeaponProjectileToHitEnemy = enemy.distanceFromPlayerGun / (PrimaryWeaponProjectile.projectileVelocity * Time.fixedDeltaTime);
                enemy.predictedPosition = MovementPrediction.EstimatePositionAfterTime(enemy.lastPosition, enemy.gameObject.transform, BasePlayer.moveVelocity, timeItWouldTakePimaryWeaponProjectileToHitEnemy);
                enemy.fastestWayForPlayerToRotateToEnemy = PlayerRotation.GetRotationTypeToFaceTarget(self, enemy.predictedPosition);
                enemy.timeForPlayerToRotateToEnemy = PlayerRotation.GetTimeRequiredToFaceTarget(self, enemy.predictedPosition);
                enemy.timeForEnemyToRotateToPlayer = PlayerRotation.GetTimeRequiredToFaceTarget(enemy.gameObject.transform, self.gameObject.transform.position);
                enemy.lastPosition = new Vector3(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y, enemy.gameObject.transform.position.z);
                enemy.priorityLevelForPlayer = this.CalculateEnemyPriorityFactor(enemy);
                Debug.DrawLine(selfGun.position, enemy.predictedPosition, Color.green);
            }
        }

        public float CalculateEnemyPriorityFactor(Enemy enemy)
        {
            float isShootableFactor = (enemy.CanBeShot() ? 1 : 0) * 0.5f; 
            // TODO SHOULD HAVE SEPARATE FACTORS FOR isBehindCover AND isOutOfShootingRange SO THEY ARE NOT LUMPED IN TOGETHER
            // TODO maybe distanceToEnemy should be multiplied by time taken to turn
            float distanceToEnemyFactor = (1f - (enemy.distanceFromPlayer / 30f)) * 0.2f;
            float timeRequiredToRotate180Degrees = (Mathf.Deg2Rad * 180) / (BasePlayer.rotationVelocity * Time.fixedDeltaTime);
            float timeForEnemyToTurnToYouFactor = (1f - (enemy.timeForEnemyToRotateToPlayer / timeRequiredToRotate180Degrees)) * 0.14f;
            float timeForSelfToTurnToEnemyFactor = (1f - (enemy.timeForPlayerToRotateToEnemy / timeRequiredToRotate180Degrees)) * 0.08f;
            // TODO amount of damage dealt to self * 0.04f
            // TODO amount of damage dealt to others * 0.02f
            // TODO amount of health remaining * 0.02f
            float totalPriorityFactor = isShootableFactor + distanceToEnemyFactor + timeForEnemyToTurnToYouFactor + timeForSelfToTurnToEnemyFactor;
            Debug.Log("SHOOTABLE " + isShootableFactor + " * DISTANCE " + distanceToEnemyFactor + " ENEMY TURN " + timeForEnemyToTurnToYouFactor + " PLAYER TURN " + timeForSelfToTurnToEnemyFactor + " = " + totalPriorityFactor);
            return totalPriorityFactor;
        }
    }
}
