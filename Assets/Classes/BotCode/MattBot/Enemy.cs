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
        public float distanceFromPlayer;
        public bool isBehindCover;
        protected float minShootingDistance;

        public Enemy(GameObject enemyGameObject)
        {
            gameObject = enemyGameObject;
            minShootingDistance = PrimaryWeaponProjectile.
        }

        public bool CanBeShot()
        {
            return (gameObject.activeSelf && !isBehindCover && distanceFromPlayer < minShootingDistance);
        }
    }
}
