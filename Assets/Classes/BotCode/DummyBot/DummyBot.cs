using UnityEngine;
using System.Collections;

namespace TestingBots
{
    /// <summary>
    /// Bot with no logic, will stand still and take damage. 
    /// Can be controlled using the unity inspector under script properties.
    /// </summary>
    public class DummyBot : BasePlayer
    {
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

        }
    }
}