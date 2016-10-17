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
    ///         - enemy spacing around
    /// </summary>
    class BulletSense : Sense
    {
        protected MattBot playerSelfScript;
        protected BulletList bulletList;

        public BulletSense (MattBot playerSelfScript, BulletList bulletList)
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
            foreach(Bullet bullet in this.bulletList.Values)
            {
                if (bullet.gameObject != null) { 
                    RaycastHit hitInfo = new RaycastHit();

                    bullet.timeToLive = (PrimaryWeaponProjectile.timeToLive - bullet.timeAlive) * 1f; // * PrimaryWeaponProjectile.projectileVelocity * PrimaryWeaponProjectile.projectileVelocity; // projectileVelocity * Time.fixedDeltaTime;
                    Debug.Log(bullet.timeToLive * PrimaryWeaponProjectile.projectileVelocity);
                    
                    
                    bullet.predictedPosition = MovementPrediction.EstimateProjectilePositionAfterTime(bullet.lastPosition, bullet.gameObject.transform, PrimaryWeaponProjectile.projectileVelocity, bullet.timeToLive); // , BasePlayer.rotationTypes.None, BasePlayer.rotationTypes.None, BasePlayer.movementTypes.Forward
                    // TODO NEED TO CONFIGURE THE LINECAST TO DETECT HITS AGAINST THE PLAYER
                    Sense.Linecast(bullet.gameObject.transform.position, bullet.predictedPosition, out hitInfo, 0, Color.red);
                    bullet.lastPosition = bullet.gameObject.transform.position;
                    bullet.timeAlive += fixedDeltaTime;
                }
            }
        }
        
    }
}
