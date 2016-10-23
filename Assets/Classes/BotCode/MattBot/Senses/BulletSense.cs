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
                    if (BulletLineCast(bullet.gameObject.transform, bullet.predictedPosition, bullet.radius, out hitInfo, 1 << LayerMask.NameToLayer("Players"), Color.red))
                    {
                        bullet.distanceFromStrikingPlayer = hitInfo.distance;
                    }
                    else
                    {
                        bullet.distanceFromStrikingPlayer = null;
                    }
                    //Debug.Log("hit " + hitInfo.distance + " proximity " + bullet.proximityToPlayer);
                    bullet.lastPosition = bullet.gameObject.transform.position;
                    bullet.timeAlive += fixedDeltaTime;
                }
            }
        }

        private bool BulletLineCast(Transform bulletTransform, Vector3 end, float bulletRadius, out RaycastHit hitInfo, int layerMask, Color debugLineColour)
        {
            // TODO WRITE OWN COLLISSION DETECTION, LINECAST SEEMS INCONSISTENT, USE DISTANCE FROM BULLET WITH PADDING
            bool leftEdge = Sense.Linecast(bulletTransform.position + bulletTransform.right * -bulletRadius, end + bulletTransform.right * -bulletRadius, out hitInfo, layerMask, debugLineColour);
            bool rightEdge = Sense.Linecast(bulletTransform.position + bulletTransform.right * bulletRadius, end + bulletTransform.right * bulletRadius, out hitInfo, layerMask, debugLineColour);
            return (leftEdge || rightEdge);
        }
    }
}
