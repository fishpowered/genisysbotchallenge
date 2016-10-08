using UnityEngine;

namespace MattBot
{
    /// <summary>
    /// Class for predicting the movement of projectiles and players
    /// </summary>
    class MovementPrediction
    {
        public static Vector3 EstimatePositionAfterTime(Vector3 lastKnownPosition, Transform currentTransform, float movementVelocity, float time)
        {
            if(lastKnownPosition == null)
            {
                return currentTransform.position;
            }

            // Get movement bearing by comparing current position to last known position
            Vector3 movementBearing = currentTransform.position - lastKnownPosition;

            // Bearing ray
            Debug.DrawRay(currentTransform.position, (movementBearing * time), Color.cyan);

            //Debug.Log("Current {"+currentTransform.position.ToString() + "} - {" + lastKnownPosition.ToString() + "} = "+ movementBearing.x);


            Vector3 predictedLocation = currentTransform.position + (movementBearing * time);
            
            return predictedLocation;
        }
    }
}
