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

        public Enemy(GameObject enemyGameObject)
        {
            gameObject = enemyGameObject;
        }
    }
}
