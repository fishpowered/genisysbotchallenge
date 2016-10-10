using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MattBot
{
    /// <summary>
    /// Helper methods for dealing with player rotation
    /// </summary>
    class PlayerRotation
    {

        public static BasePlayer.rotationTypes GetRotationTypeToFaceTarget(Transform playerSelf, Vector3 target)
        {
            float angle = GetAngleRequiredToTurnToFaceTarget(playerSelf, target, false);
            if (angle >= 0f)
            {
                return BasePlayer.rotationTypes.Left;
            }
            else
            {
                return BasePlayer.rotationTypes.Right;
            }
        }

        public static float GetAngleRequiredToTurnToFaceTarget(Transform playerSelf, Vector3 target, bool absoluteAngle)
        {
            Vector3 playerForwardFacingVector = playerSelf.transform.position + (playerSelf.transform.forward); // need to get the forward facing vector so we know the direction the player is facing
            // TODO NEED TO ACCOUNT FOR GUN BEING OUTSIDE OF PLAYER BODY
            float returnAngle = Vector3.Angle(playerSelf.transform.position - target, playerForwardFacingVector - playerSelf.transform.position) - 180f;

            if (absoluteAngle)
            {
                return Mathf.Abs(returnAngle);
            }
            else
            {
                Vector3 playerRightFacingVector = playerSelf.transform.position + (playerSelf.transform.right * 2f);
                Vector3 playerLeftFacingVector = playerSelf.transform.position + (playerSelf.transform.right * -2f);
                //Debug.DrawLine(playerSelf.position, playerForwardFacingVector, Color.magenta);
                //Debug.DrawLine(playerSelf.position, playerRightFacingVector, Color.magenta);
                //Debug.DrawLine(playerSelf.position, playerLeftFacingVector, Color.magenta);
                if (Vector3.Distance(playerRightFacingVector, target) > Vector3.Distance(playerLeftFacingVector, target))
                {
                    return returnAngle * -1f;
                }
                else
                {
                    return returnAngle;
                }
            }
        }

        public static float GetTimeRequiredToFaceTarget(Transform playerSelf, Vector3 target)
        {
            float angleRequiredToTurn = GetAngleRequiredToTurnToFaceTarget(playerSelf, target, true);
            return (Mathf.Deg2Rad * angleRequiredToTurn) / (BasePlayer.rotationVelocity * Time.fixedDeltaTime);
        }
    }
}
