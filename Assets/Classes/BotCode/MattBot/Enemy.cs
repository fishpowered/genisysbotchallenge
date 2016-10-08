using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    class Enemy
    {
        public GameObject gameObject;
        public GameObject gunGameObject;
        public BasePlayer playerScript;
        public float distanceFromPlayer;
        public Vector3 lastPosition;

        /// <summary>
        /// Distance from our bot player gun to the enemy
        /// </summary>
        public float distanceFromPlayerGun;
        public bool isBehindCover;
        protected float minShootingDistance;

        public Enemy(GameObject enemyGameObject)
        {
            gameObject = enemyGameObject;
            gunGameObject = gameObject.transform.Find("Gun").gameObject;
            playerScript = gameObject.GetComponent<BasePlayer>();
            minShootingDistance = (PrimaryWeaponProjectile.projectileVelocity * PrimaryWeaponProjectile.timeToLive) + 1f;
        }

        public bool CanBeShot()
        {
            return (gameObject.activeSelf && !isBehindCover && distanceFromPlayer < minShootingDistance);
        }
    }
}
