using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MattBot
{
    /// <summary>
    /// Responsible for detecting where enemies
    /// </summary>
    public class EnemySense
    {
        protected Transform self;
        protected List<Transform> enemyPlayerTransforms;
        public bool spotted;
        public EnemySense(Transform self, List<Transform> enemyPlayers)
        {
            this.enemyPlayerTransforms = enemyPlayers;
            this.self = self;
        }

        public void check()
        {
            foreach(Transform enemyPlayer in enemyPlayerTransforms)
            {
                spotted = Physics.Linecast(self.position, enemyPlayer.position, 1 << LayerMask.NameToLayer("Players"));
                
                if (spotted)
                {
                    Debug.DrawLine(self.position, enemyPlayer.position, Color.red, Time.fixedDeltaTime);
                    //Debug.Log("Spotted "+enemyPlayer.gameObject.name+" Drawing line from " + enemyPlayer.position.ToString() + " to " + self.position.ToString());
                    Debug.Log("Spotted " + enemyPlayer.gameObject.name + "Distance" + Vector3.Distance(self.position, enemyPlayer.position));
                }
            }
        }
    }
}
