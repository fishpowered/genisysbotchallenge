using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    class BulletList : Dictionary<GameObject, Bullet>
    {
        protected GameObject playerSelf;

        public BulletList(GameObject playerSelf)
        {
            this.playerSelf = playerSelf;
        }

        /// <summary>
        /// Update list with the bullets that are flying about
        /// </summary>
        public void UpdateList()
        {
            // Look for new bullets
            GameObject[] bulletProjectileGameObjects = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject bulletGameObject in bulletProjectileGameObjects)
            {
                if(this.ContainsKey(bulletGameObject)==false)
                {
                    Bullet bullet = new Bullet(bulletGameObject);
                    if(bullet.baseScript.source != this.playerSelf) {
                        this.Add(bulletGameObject, bullet);
                    }
                }
            }

            // Check for dead bullets
           // foreach (Bullet bullet in this.Values)
            //{
                //if(bullet.gameObject == null)
                //{
                //    Remove(bullet.gameObject);
                //}
            //}
            //Debug.Log("BULLET COUNT " + this.Count);
        }

        /// <summary>
        /// Check all bullets heading towards the player and return the closest
        /// </summary>
        /// <returns></returns>
        public Bullet GetClosestBulletToStrikingPlayerSelf()
        {
            Bullet closestBullet = null;
            foreach (Bullet bullet in this.Values)
            {
                if(bullet.gameObject != null && bullet.distanceFromStrikingPlayer!=null && (closestBullet == null || bullet.distanceFromStrikingPlayer < closestBullet.distanceFromStrikingPlayer))
                {
                    closestBullet = bullet;
                }
            }
            if(closestBullet!= null){
                return closestBullet;
            }else{
                return null;
            }
            
        }

        /// <summary>
        /// Populate list with all enemy players
        /// </summary>
        /*public Enemy GetEnemyWithHighestPriorityLevel()
        {
            Enemy enemyToReturn = null;
            foreach (Enemy enemy in this)
            {
                if(enemy.IsAlive() && (enemyToReturn == null || enemy.priorityLevelForPlayer > enemyToReturn.priorityLevelForPlayer ))
                {
                    enemyToReturn = enemy;
                }
            }
            return enemyToReturn;
        }*/
    }
}
