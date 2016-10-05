using UnityEngine;
using System.Collections;

namespace TestingBots { 

    /// <summary>
    /// This is a test player that can be controlled by a human player to for testing purposes.
    /// Use WASD keys to strafe, arrow left and right to rotate, and the space key to shoot.
    /// </summary>
    public class HumanControlledBot : BasePlayer {

        /// <summary>
        /// Called every physics cycle. This is where your bot logic can be written. 
        /// Should always call UpdatePlayerXxx methods but the order these occur in is up to you.
        /// </summary>
        protected override void UpdatePlayerState()
        {
            CheckPlayerInput();
            UpdatePlayerMovement();
            UpdatePlayerRotation();
            UpdatePlayerShootingState();
        }

        /// <summary>
        /// As this is a human controlled bot, we need to check for keyboard inputs and translate that directly into
        /// move and shoot commands
        /// </summary>
        protected void CheckPlayerInput()
        {
            bool forwardKey = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            bool backwardKey = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            bool strafeLeftKey = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.PageUp);
            bool strafeRightKey = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.PageDown);
            bool rotateLeftKey = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q);
            bool rotateRightKey = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.E);
            bool shootKey = Input.GetKey(KeyCode.Space);

            // Check for shooting commands
            if (shootKey == true)
            {
                ShootPrimaryWeapon();
            }

            // Check for movement commands
            if (forwardKey == true && strafeLeftKey == true)
            {
                movePlayer = movementTypes.ForwardAndLeft;
            }
            else if (forwardKey == true && strafeRightKey == true)
            {
                movePlayer = movementTypes.ForwardAndRight;
            }
            else if (backwardKey == true && strafeLeftKey == true)
            {
                movePlayer = movementTypes.BackAndLeft;
            }
            else if (backwardKey == true && strafeRightKey == true)
            {
                movePlayer = movementTypes.BackAndRight;
            }
            else if (forwardKey == true)
            {
                movePlayer = movementTypes.Forward;
            }
            else if (strafeLeftKey == true)
            {
                movePlayer = movementTypes.Left;
            }
            else if (strafeRightKey == true)
            {
                movePlayer = movementTypes.Right;
            }
            else if (backwardKey == true)
            {
                movePlayer = movementTypes.Back;
            }
            else
            {
                movePlayer = movementTypes.None;
            }

            // Check for rotation commands
            if (rotateRightKey == true)
            {
                rotatePlayer = rotationTypes.Right;
            }
            else if (rotateLeftKey == true)
            {
                rotatePlayer = rotationTypes.Left;
            }
            else
            {
                rotatePlayer = rotationTypes.None;
            }
        }
    }
}