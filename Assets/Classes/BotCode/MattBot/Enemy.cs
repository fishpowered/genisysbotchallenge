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
        protected float minShootingDistance;

        // Information accumulated from senses...
        public float distanceFromPlayer;
        public float distanceFromPlayerGun;
        public Vector3 lastPosition;
        public Vector3 predictedPosition;
        public BasePlayer.rotationTypes fastestWayForPlayerToRotateToEnemy;
        public float timeForPlayerToRotateToEnemy;
        public float timeForEnemyToRotateToPlayer;
        public bool isBehindCover;
        public float priorityLevelForPlayer;
        
        public Enemy(GameObject enemyGameObject)
        {
            gameObject = enemyGameObject;
            gunGameObject = gameObject.transform.Find("Gun").gameObject;
            playerScript = gameObject.GetComponent<BasePlayer>();
            minShootingDistance = (PrimaryWeaponProjectile.projectileVelocity * PrimaryWeaponProjectile.timeToLive) + 1f;
        }

        public bool CanBeShot()
        {
            return (IsAlive() && !isBehindCover && distanceFromPlayer < minShootingDistance);
        }

        public bool IsAlive()
        {
            return (gameObject.activeSelf);
        }
    }
}
