using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{
    /// <summary>
    /// Responsible for detecting where enemies
    /// </summary>
    class EnemySense : Sense
    {
        protected Transform self;
        protected EnemyList enemyList;
        public bool enemyInCover;
        public EnemySense(Transform self, EnemyList enemyList)
        {
            this.enemyList = enemyList;
            this.self = self;
        }

        public void check()
        {
            foreach(Enemy enemy in enemyList)
            {
                RaycastHit hitInfo = new RaycastHit();
                enemy.isBehindCover = Sense.Linecast(self.position, enemy.gameObject.transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Environment"), Color.red);
                enemy.distanceFromPlayer = Vector3.Distance(self.position, enemy.gameObject.transform.position);


                if (enemy.isBehindCover)
                {
                    //  Debug.Log("HIT" + (hitInfo.collider.gameObject.name));
                    //  Debug.DrawLine(self.position, enemy.gameObject.transform.position, Color.red, Time.fixedDeltaTime);
                    //Debug.Log("Spotted "+enemyPlayer.gameObject.name+" Drawing line from " + enemyPlayer.position.ToString() + " to " + self.position.ToString());
                    //Debug.Log("Spotted " + enemy.gameObject.name + "Distance" + Vector3.Distance(self.position, enemy.gameObject.transform.position));
                }
            }
        }
    }
}
