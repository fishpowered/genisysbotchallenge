﻿using UnityEngine;

namespace MattBot
{
    /// <summary>
    /// Class for predicting the movement of projectiles and players
    /// </summary>
    class MovementPrediction
    {
        public static Vector3 EstimatePositionAfterTime(Vector3 lastKnownPosition, Transform currentTransform, float movementVelocity, float time, BasePlayer.rotationTypes lastKnownRotation, BasePlayer.rotationTypes currentRotation, BasePlayer.movementTypes predictedMovementType)
        {
            if (lastKnownPosition == null)
            {
                return currentTransform.position;
            }

            // Get movement bearing by comparing current position to last known position
            Vector3 movementBearing = currentTransform.position - lastKnownPosition;

            // Bearing ray
            Debug.DrawRay(currentTransform.position, (movementBearing * time), Color.cyan);

            //Debug.Log("Current {"+currentTransform.position.ToString() + "} - {" + lastKnownPosition.ToString() + "} = "+ movementBearing.x);


            Vector3 predictedLocation = currentTransform.position + (movementBearing * time);

            // Adjust prediction based on predicted rotation if rotation seems consistent
            float tweakFactor = (movementBearing.magnitude * time) / 2f;
            if (lastKnownRotation == currentRotation && lastKnownPosition != currentTransform.position && currentRotation != BasePlayer.rotationTypes.None)
            {
                switch (predictedMovementType)
                {
                    case BasePlayer.movementTypes.None:
                        break;
                    case BasePlayer.movementTypes.Forward:
                        switch (currentRotation)
                        {
                            case BasePlayer.rotationTypes.Left:
                                predictedLocation += (currentTransform.right * tweakFactor) * -1f;
                                break;
                            case BasePlayer.rotationTypes.Right:
                                predictedLocation += (currentTransform.right * tweakFactor);
                                break;
                        }
                        break;
                    case BasePlayer.movementTypes.Back:
                        switch (currentRotation)
                        {
                            case BasePlayer.rotationTypes.Left:
                                predictedLocation += (currentTransform.right * tweakFactor);
                                break;
                            case BasePlayer.rotationTypes.Right:
                                predictedLocation += (currentTransform.right * tweakFactor) * -1f;
                                break;
                        }
                        break;
                    case BasePlayer.movementTypes.Left:
                        switch (currentRotation)
                        {
                            case BasePlayer.rotationTypes.Left:
                                predictedLocation += ((currentTransform.right + (currentTransform.forward * -1f)) * tweakFactor);
                                break;
                            case BasePlayer.rotationTypes.Right: // moving left and rotating right, so predicted location should be forward and right to where it was previously
                                predictedLocation += ((currentTransform.forward + currentTransform.right) * tweakFactor);
                                break;
                        }
                        break;
                    case BasePlayer.movementTypes.Right:
                        switch (currentRotation)
                        {
                            case BasePlayer.rotationTypes.Right: // moving right and rotating right, so predicted position should be back and left
                                predictedLocation += ((-currentTransform.right + -currentTransform.forward)) * tweakFactor;
                                break;
                            case BasePlayer.rotationTypes.Left: // moving right and rotating left, so predicted position should be forward and left
                                predictedLocation += ((currentTransform.forward - currentTransform.right) * tweakFactor);
                                break;
                        }
                        break;
                }
            }

            return predictedLocation;
        }
    }
}