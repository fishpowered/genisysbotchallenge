using UnityEngine;
using System.Collections;

namespace TestingBots
{
    /// <summary>
    /// Simple bot that will run forwards, randomly turn left and right, and shoot
    /// </summary>
    public class AngryHoboBot : BasePlayer
    {
        /// <summary>
        /// Keep track of how much time has elapsed
        /// </summary>
        protected float timeLeftBeforeChangeOfDirection = 1f;

        /// <summary>
        /// Called once after bot is spawned. This is for intialising your bot code.
        /// </summary>
        protected override void InitPlayer()
        {

        }

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// </summary>
        protected override void UpdatePlayerState()
        {
            // Tell player to move forward and shoot if possible
            movePlayer = movementTypes.Forward;
            if (CanShootPrimaryWeapon())
            {
                shootPrimaryWeapon = true;
            }

            // Tell player to switch between left and right directions after a while
            if (timeLeftBeforeChangeOfDirection <= 0)
            {
                if(rotatePlayer == rotationTypes.Right)
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
