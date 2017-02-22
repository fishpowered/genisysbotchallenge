using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

namespace MattBot
{
    /// <summary>
    /// Responsible for detecting where bullets are heading and whether they will hit the player.
    /// 
    /// Attack/defense plan:
    /// - try and stay out of enemy firing range unless enemy reloading or facing the wrong way?
    /// - shoot from cover?
    /// Priorities:
    /// - dodge anything that is going to hit player (wait till last possible moment to reduce chance of spread)
    ///     - direction to dodge weighted factors:
    ///         - how far player can move in a particular direction (freedom/space)
    ///         - likeliness to hit a projectile
    ///             - rather than copying player gameobject, just use a simple radius check
    ///         - enemy spacing around
    /// </summary>
    class BulletSense : Sense
    {
        protected MattBot playerSelfScript;
        protected BulletList bulletList;

        //protected int 
        public BulletSense(MattBot playerSelfScript, BulletList bulletList)
        {

            this.playerSelfScript = playerSelfScript;
            this.bulletList = bulletList;

        }

        /// <summary>
        /// Update bullet status
        /// </summary>
        public void Update()
        {

            float fixedDeltaTime = Time.fixedDeltaTime;
            foreach (Bullet bullet in this.bulletList.Values)
            {
                if (bullet.gameObject != null)
                {
                    RaycastHit hitInfo = new RaycastHit();

                    bullet.timeToLive = (PrimaryWeaponProjectile.timeToLive - bullet.timeAlive); // * PrimaryWeaponProjectile.projectileVelocity * PrimaryWeaponProjectile.projectileVelocity; // projectileVelocity * Time.fixedDeltaTime;
                    bullet.proximityToPlayer = Vector3.Distance(bullet.gameObject.transform.position, playerSelfScript.gameObject.transform.position);
                    if (bullet.NeedsTradjectoryChecking()) { 
                        //hitInfo.
                        bullet.predictedPosition = MovementPrediction.EstimateProjectilePositionAfterTime(bullet.lastPosition, bullet.gameObject.transform, PrimaryWeaponProjectile.projectileVelocity, bullet.timeToLive); // , BasePlayer.rotationTypes.None, BasePlayer.rotationTypes.None, BasePlayer.movementTypes.Forward
                    
                        float timeTillHit = TimeItWillTakeForBulletToCollideWithPlayer(bullet, playerSelfScript.transform.position, PrimaryWeaponProjectile.projectileVelocity * Time.fixedDeltaTime);
                        //Debug.Log(bullet.timeToLive);
                        if (timeTillHit > -0.5f)
                        {
                            bullet.distanceFromStrikingPlayer = timeTillHit;
                        }
                    }
                    //Debug.Log("hit " + hitInfo.distance + " proximity " + bullet.proximityToPlayer);
                    bullet.lastPosition = bullet.gameObject.transform.position;
                    bullet.timeAlive += fixedDeltaTime;
                    bullet.lastProximityToPlayer = bullet.proximityToPlayer;
                }
            }
        }

        public static bool WillCurrentBulletPositionCollideWithCurrentPlayerPosition(Vector3 bulletPosition, float bulletRadius, float bulletVelocity, Vector3 playerPosition, float playerRadius)
        {
            if(Vector3.Distance(bulletPosition, playerPosition) - bulletRadius < playerRadius)
            {
                return true;
            }
            return false;
        }

        public static float TimeItWillTakeForBulletToCollideWithPlayer(Bullet bullet, Vector3 playerPosition, float bulletVelocity)
        {
            float timeAlive = 0f;
            float timeStep = Sense.predictionStep;
            float timeToLive = bullet.timeToLive;
            while (bullet.gameObject != null && bullet.gameObject.activeSelf && timeAlive < timeToLive)
            {
                timeAlive += timeStep;
                // TODO PREDICT PLAYERS MOVEMENT AS WELL BASED ON MOVEMENT DIRECTION WE WANT TO SIMULATE
                Vector3 bulletTrajectoryStep = MovementPrediction.EstimateProjectilePositionAfterTime(bullet.lastPosition, bullet.gameObject.transform, bulletVelocity, timeAlive);
                if (WillCurrentBulletPositionCollideWithCurrentPlayerPosition(bulletTrajectoryStep, bullet.radius, bulletVelocity, playerPosition, MattBot.playerRadius))
                {
                    Debug.DrawLine(bulletTrajectoryStep, bulletTrajectoryStep * 0.98f, Color.red);
                    return timeAlive * PrimaryWeaponProjectile.projectileVelocity;
                }
                else
                {
                    Debug.DrawLine(bulletTrajectoryStep, bulletTrajectoryStep * 0.98f, Color.green);
                }
            }
            return -999f;
        }
    }
}
