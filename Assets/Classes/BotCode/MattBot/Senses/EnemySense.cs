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

        public void Check()
        {
            foreach(Enemy enemy in enemyList)
            {
                RaycastHit hitInfo = new RaycastHit();
                enemy.isBehindCover = Sense.Linecast(self.position, enemy.gameObject.transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Environment"), Color.red);
                enemy.distanceFromPlayer = Vector3.Distance(self.position, enemy.gameObject.transform.position);
                enemy.distanceFromPlayerGun = Vector3.Distance(selfGun.position, enemy.gameObject.transform.position);
                

                float timeItWouldTakePimaryWeaponProjectileToHitEnemy = enemy.distanceFromPlayerGun / (PrimaryWeaponProjectile.projectileVelocity * Time.fixedDeltaTime);
                Vector3 predictedPosition;
                
                predictedPosition = MovementPrediction.EstimatePositionAfterTime(enemy.lastPosition, enemy.gameObject.transform, BasePlayer.moveVelocity, timeItWouldTakePimaryWeaponProjectileToHitEnemy);
                

                Debug.DrawLine(selfGun.position, predictedPosition, Color.green);

                if (enemy.CanBeShot())
                {
                    selfPlayerScript.shootPrimaryWeapon = true;
                    //  Debug.Log("HIT" + (hitInfo.collider.gameObject.name));
                    //  Debug.DrawLine(self.position, enemy.gameObject.transform.position, Color.red, Time.fixedDeltaTime);
                    //Debug.Log("Spotted "+enemyPlayer.gameObject.name+" Drawing line from " + enemyPlayer.position.ToString() + " to " + self.position.ToString());
                    //Debug.Log("Spotted " + enemy.gameObject.name + "Distance" + Vector3.Distance(self.position, enemy.gameObject.transform.position));
                }else
                {
                    selfPlayerScript.shootPrimaryWeapon = false;
                }
                enemy.lastPosition = new Vector3(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y, enemy.gameObject.transform.position.z);
            }
        }
    }
}
