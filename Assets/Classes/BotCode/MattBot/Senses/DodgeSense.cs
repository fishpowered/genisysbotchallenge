using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace MattBot
{
    class DodgeSense : Sense
    {
        protected MattBot playerSelfScript;
        protected BulletList bulletList;

        public DodgeSense(MattBot selfPlayerScript, BulletList bulletList)
        {
            this.playerSelfScript = selfPlayerScript;
            this.bulletList = bulletList;
        }

        public BasePlayer.movementTypes DetermineBestDirectionForPlayerSelfToMove()
        {
            List<DodgeAttemptPrediction> dodgeAttemptList = new List<DodgeAttemptPrediction>();
            foreach (BasePlayer.movementTypes movementType in System.Enum.GetValues(typeof(BasePlayer.movementTypes)))
            {
                DodgeAttemptPrediction dodgeAttempt = new DodgeAttemptPrediction();
                dodgeAttempt.movementType = movementType;
                dodgeAttempt.maxDistancePlayerCanMove = 10f; // TODO use raycast to test how far player can move
                while (dodgeAttempt.durationTested < dodgeAttempt.maxDurationToTest && dodgeAttempt.hasFoundSafeSpot == false && dodgeAttempt.distancePlayerHasMoved < dodgeAttempt.maxDistancePlayerCanMove)
                {
                    dodgeAttempt.durationTested += Sense.predictionStep;
                    // testc for whether the player will get hit in the time and eventually

                    Vector3 predictedPlayerPosition = MovementPrediction.GetProjectedPlayerPositionAfterTime(playerSelfScript.transform, movementType, dodgeAttempt.durationTested);
                    foreach (Bullet bullet in this.bulletList.Values)
                    {
                        if (bullet.gameObject != null)
                        {
                            float timeAtWhichBulletWillHitPlayerPosition = BulletSense.TimeItWillTakeForBulletToCollideWithPlayer(bullet, predictedPlayerPosition, PrimaryWeaponProjectile.projectileVelocity * Time.fixedDeltaTime);
                            if (timeAtWhichBulletWillHitPlayerPosition < -1f)
                            {
                                // Bullet won't hit at all
                                //Debug.DrawLine(predictedPlayerPosition, predictedPlayerPosition * 0.97f, Color.green);
                                //dodgeAttempt.hasFoundSafeSpot = true;
                            }
                            else if ((dodgeAttempt.durationTested - timeAtWhichBulletWillHitPlayerPosition) < 0.01f)
                            {
                                // Bullet will be a direct hit in the time it has taken to move there
                                dodgeAttempt.possibleHitsTaken++;
                                Debug.DrawLine(predictedPlayerPosition, predictedPlayerPosition * 0.97f, Color.red);
                            }
                            else
                            {
                                // Bullet won't hit when the player is there but will eventually hit
                                dodgeAttempt.possibleHitsTaken++;
                                Debug.DrawLine(predictedPlayerPosition, predictedPlayerPosition * 0.97f, Color.blue);
                            }
                        }
                    }
                }
                dodgeAttemptList.Add(dodgeAttempt);
            }
            DodgeAttemptPrediction bestPrediction = null;
            foreach(DodgeAttemptPrediction dodgeAttempt in dodgeAttemptList)
            {
                if(bestPrediction==null || dodgeAttempt.possibleHitsTaken < bestPrediction.possibleHitsTaken)
                {
                    bestPrediction = dodgeAttempt;
                }
            }
            return bestPrediction.movementType;
        }


        
    }

    class DodgeAttemptPrediction
    {
        public BasePlayer.movementTypes movementType;
        public float durationTested = 0f;
        public float maxDurationToTest = 3f;
        public bool hasFoundSafeSpot = false;
        public float distancePlayerHasMoved = 0f;
        public float maxDistancePlayerCanMove = 10f;
        public int possibleHitsTaken = 0;
    }
}
