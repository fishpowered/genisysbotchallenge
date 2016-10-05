using UnityEngine;
using System.Collections;

namespace TestingBots
{
    /// <summary>
    /// Bot with no logic, will stand still and take damage. 
    /// Can be moved using the unity inspector under script properties.
    /// </summary>
    public class DummyBot : BasePlayer
    {

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// Should always call UpdatePlayerXxx methods but the order these occur in is up to you.
        /// </summary>
        protected override void UpdatePlayerState()
        {
            UpdatePlayerMovement();
            UpdatePlayerRotation();
            UpdatePlayerShootingState();
        }
    }
}