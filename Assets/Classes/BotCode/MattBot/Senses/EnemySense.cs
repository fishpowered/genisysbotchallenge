using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{
    /// <summary>
    /// Responsible for detecting where enemies
    /// </summary>
    class EnemySense
    {
        protected Transform self;
        protected EnemyList enemyList;
        public bool spotted;
        public EnemySense(Transform self, EnemyList enemyList)
        {
            this.enemyList = enemyList;
            this.self = self;
        }

        public void check()
        {
            foreach(Enemy enemy in enemyList)
            {
                spotted = Physics.Linecast(self.position, enemy.gameObject.transform.position, 1 << LayerMask.NameToLayer("Players"));
                
                if (spotted)
                {
                    Debug.DrawLine(self.position, enemy.gameObject.transform.position, Color.red, Time.fixedDeltaTime);
                    //Debug.Log("Spotted "+enemyPlayer.gameObject.name+" Drawing line from " + enemyPlayer.position.ToString() + " to " + self.position.ToString());
                    Debug.Log("Spotted " + enemy.gameObject.name + "Distance" + Vector3.Distance(self.position, enemy.gameObject.transform.position));
                }
            }
        }
    }
}
