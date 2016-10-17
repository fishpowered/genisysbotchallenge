using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    class Sense
    {
        public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask, Color debugLineColour)
        {
            bool collisionResult = Physics.Linecast(start, end, out hitInfo, layerMask);
            Debug.DrawLine(start, end, debugLineColour);

            if (collisionResult)
            {
                Debug.DrawLine(start, end, debugLineColour); // Debug.Log("HIT" + (hitInfo.collider.gameObject.name));
                //Debug.Log("Spotted "+enemyPlayer.gameObject.name+" Drawing line from " + enemyPlayer.position.ToString() + " to " + self.position.ToString());
                // Debug.Log("Spotted " + enemy.gameObject.name + "Distance" + Vector3.Distance(self.position, enemy.gameObject.transform.position));
            }
            return collisionResult;
        }
    }
}
