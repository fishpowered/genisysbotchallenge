using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    class Bullet
    {
        public GameObject gameObject;
        public PrimaryWeaponProjectile baseScript;
        protected float minShootingDistance;

        // Information accumulated from senses...
        public Nullable<float> distanceFromStrikingPlayer;
        public float proximityToPlayer;
        //public float distanceFromPlayerGun;
        public float timeToLive;
        public float timeAlive = 0f;
        public Vector3 lastPosition;
        public Vector3 predictedPosition;
        public float radius;
        /*public BasePlayer.rotationTypes lastRotation;
        public BasePlayer.rotationTypes predictedRotation;
        public BasePlayer.rotationTypes fastestWayForPlayerToRotateToEnemy;
        public BasePlayer.movementTypes predictedMovementDirection;
        public BasePlayer.movementTypes lastdMovementDirection;
        public float timeForPlayerToRotateToEnemy;
        public float timeForEnemyToRotateToPlayer;
        public bool isBehindCover;
        public float priorityLevelForPlayer;
        */
        public Bullet(GameObject bulletGameObject)
        {
            gameObject = bulletGameObject;
            baseScript = gameObject.GetComponent<PrimaryWeaponProjectile>();
            minShootingDistance = (PrimaryWeaponProjectile.projectileVelocity * PrimaryWeaponProjectile.timeToLive) + 1f;
            radius = gameObject.GetComponent<Renderer>().bounds.size.x / 2f;
        }

        public bool IsAlive()
        {
            return (gameObject.activeSelf);
        }
    }
}
