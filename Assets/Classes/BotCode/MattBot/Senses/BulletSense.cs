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
                    if (DEPRECATED_BulletLineCast(bullet.gameObject.transform, bullet.predictedPosition, bullet.radius, out hitInfo, 1 << LayerMask.NameToLayer("Players"), Color.red))
                    {
                        bullet.distanceFromStrikingPlayer = hitInfo.distance;
                    }
                    else
                    {
                        bullet.distanceFromStrikingPlayer = null;
                    }
                    if(WillCurrentBulletPositionCollideWithCurrentPlayerPosition(bullet.gameObject.transform.position, bullet.radius, PrimaryWeaponProjectile.projectileVelocity, playerSelfScript.gameObject.transform.position, 1.1f))
                    {
                        Debug.Log("PREDICTED HIT");
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

        public static bool WillBulletTrajectoryCollideWithPlayerTrajectory(Bullet bullet)
        {
            while (bullet.gameObject != null && bullet.gameObject.activeSelf && bullet.timeToLive > 0.1f)
            {
                // TODO LOOP THROUGH AND CHECK WillCurrentBulletPositionCollideWithCurrentPlayerPosition WITH EstimateProjectilePositionAfterTime 
            }
            return false;
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
