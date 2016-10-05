using UnityEngine;
using System.Collections;

namespace MattsBotCode
{

    public class MattBot : BasePlayer
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
