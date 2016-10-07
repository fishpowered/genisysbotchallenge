using UnityEngine;
using System.Collections;

namespace MattsBotCode
{

    public class MattBot : BasePlayer
    {

        /// <summary>
        /// Called once after bot is spawned. This is for intialising your bot code.
        /// </summary>
        protected override void InitPlayer()
        {

        }

        /// <summary>
        /// Keep track of how much time has elapsed
        /// </summary>
        protected float timeLeftBeforeChangeOfDirection = 1f;

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// </summary>
        protected override void UpdatePlayerState()
        {
            // Tell player to move forward and shoot if possible
            movePlayer = movementTypes.None;
            shootPrimaryWeapon = true;

            // Tell player to switch between left and right directions after a while
            if (timeLeftBeforeChangeOfDirection <= 0)
            {
                if (rotatePlayer == rotationTypes.Right)
                {
                    rotatePlayer = rotationTypes.Left;
                }
                else
                {
                    rotatePlayer = rotationTypes.Right;
                }
                timeLeftBeforeChangeOfDirection = Random.Range(0.5f, 3.5f);

            }
            else
            {
                timeLeftBeforeChangeOfDirection -= Time.fixedDeltaTime;
            }
        }
    }
}
