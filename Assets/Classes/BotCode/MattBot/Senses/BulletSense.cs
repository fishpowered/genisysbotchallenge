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

                    //hitInfo.
                    bullet.predictedPosition = MovementPrediction.EstimateProjectilePositionAfterTime(bullet.lastPosition, bullet.gameObject.transform, PrimaryWeaponProjectile.projectileVelocity, bullet.timeToLive); // , BasePlayer.rotationTypes.None, BasePlayer.rotationTypes.None, BasePlayer.movementTypes.Forward
                    bullet.proximityToPlayer = Vector3.Distance(bullet.gameObject.transform.position, playerSelfScript.gameObject.transform.position);
                   
                    float timeTillHit = TimeBulletTrajectoryCollideWithPlayerTrajectory(bullet, playerSelfScript, PrimaryWeaponProjectile.projectileVelocity * Time.fixedDeltaTime);
                    //Debug.Log(bullet.timeToLive);
                    if (timeTillHit > -0.5f)
                    {
                        bullet.distanceFromStrikingPlayer = timeTillHit;
                    }
                        //Debug.Log("hit " + hitInfo.distance + " proximity " + bullet.proximityToPlayer);
                        bullet.lastPosition = bullet.gameObject.transform.position;
                    bullet.timeAlive += fixedDeltaTime;
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

        public static float TimeBulletTrajectoryCollideWithPlayerTrajectory(Bullet bullet, MattBot playerSelfScript, float bulletVelocity)
        {
            
            float timeAlive = 0f;
            float timeStep = (Time.fixedDeltaTime * 10f);
            float timeToLive = bullet.timeToLive;
            while (bullet.gameObject != null && bullet.gameObject.activeSelf && timeAlive < timeToLive)
            {
                timeAlive += timeStep;
                timeToLive -= timeStep;
                // TODO PREDICT PLAYERS MOVEMENT AS WELL BASED ON MOVEMENT DIRECTION WE WANT TO SIMULATE
                Vector3 bulletTrajectoryStep = MovementPrediction.EstimateProjectilePositionAfterTime(bullet.lastPosition, bullet.gameObject.transform, bulletVelocity, timeAlive);
                Debug.DrawLine(bulletTrajectoryStep, bulletTrajectoryStep*0.98f, Color.red);
                if (WillCurrentBulletPositionCollideWithCurrentPlayerPosition(bulletTrajectoryStep, bullet.radius, bulletVelocity, playerSelfScript.gameObject.transform.position, MattBot.playerRadius))
                {
                    return timeAlive * PrimaryWeaponProjectile.projectileVelocity;
                }
                
                
            }
            return -1f;
        }

        private bool DEPRECATED_BulletLineCast(Transform bulletTransform, Vector3 end, float bulletRadius, out RaycastHit hitInfo, int layerMask, Color debugLineColour)
        {
            // TODO WRITE OWN COLLISSION DETECTION, LINECAST SEEMS INCONSISTENT, ALSO THIS WILL TRIGGER WITH COLLISION AGAINST ANY BOT LOL, USE DISTANCE FROM BULLET WITH PADDING
            bool leftEdge = Sense.Linecast(bulletTransform.position + bulletTransform.right * -bulletRadius, end + bulletTransform.right * -bulletRadius, out hitInfo, layerMask, debugLineColour);
            bool rightEdge = Sense.Linecast(bulletTransform.position + bulletTransform.right * bulletRadius, end + bulletTransform.right * bulletRadius, out hitInfo, layerMask, debugLineColour);
            return (leftEdge || rightEdge);
        }
    }
}
