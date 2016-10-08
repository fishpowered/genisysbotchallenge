using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Base class for all simple player actions. Bot A.I code should derive from this and implement UpdatePlayerState()
/// </summary>
abstract public class BasePlayer : MonoBehaviour {

    /// <summary>
    /// Supported player movement types
    /// </summary>
    public enum movementTypes { None, Forward, Back, Left, Right, ForwardAndLeft, ForwardAndRight, BackAndLeft, BackAndRight };

    /// <summary>
    /// Current player move instruction
    /// </summary>
    public movementTypes movePlayer;

    /// <summary>
    /// Supported player rotation types
    /// </summary>
    public enum rotationTypes { None, Left, Right }

    /// <summary>
    /// Current player rotation instruction
    /// </summary>
    public rotationTypes rotatePlayer;

    /// <summary>
    /// Forward/back/left/right player movement velocity.
    /// </summary>
    public const float moveVelocity = 2f;

    /// <summary>
    /// Player rotation velocity.
    /// </summary>
    public const float rotationVelocity = 100f;

    /// <summary>
    /// Primary weapon projectile velocity.
    /// </summary>
    public const float primaryWeaponMinRefireTime = 0.2f;

    /// <summary>
    /// Primary weapon reload time.
    /// </summary>
    public const float primaryWeaponReloadTime = 2f;

    /// <summary>
    /// Primary weapon magazine size.
    /// </summary>
    public const int primaryWeaponMagazineSize = 5;

    /// <summary>
    /// GameObject that represents the player's primary weapon. Projectiles are fired from this transform.position
    /// </summary>
    protected GameObject primaryWeapon;

    /// <summary>
    /// If player shoots, a Bullet prefab will be spawned and stored here
    /// </summary>
    private GameObject primaryWeaponProjectilePrefab;

    /// <summary>
    /// If true the player will attempt to shoot. See ShootPrimaryWeapon()
    /// </summary>
    public bool shootPrimaryWeapon = false;

    /// <summary>
    /// Keeps track of time between shots of primary weapon
    /// </summary>
    private float primaryWeaponShotTimer;

    /// <summary>
    /// Keeps track of shots fired from current magazine
    /// </summary>
    private float primaryWeaponShotCounter;

    /// <summary>
    /// How long has the weapon been reloading for
    /// </summary>
    private float primaryWeaponReloadTimer = 0f;

    /// <summary>
    /// Player health
    /// </summary>
    private int health = 100;

    /// <summary>
    /// Initialise player object
    /// </summary>
    void Start () {
        primaryWeaponProjectilePrefab = Resources.Load("PrimaryWeaponProjectile") as GameObject;
        primaryWeapon = gameObject.transform.Find("Gun").gameObject;

        // Spawn the player off the floor
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 3f, gameObject.transform.position.z);
        if(gameObject.tag != "Player")
        {
            throw new UnityException("Player must be tagged as 'Player'");
        }
        gameObject.GetComponentInChildren<Text>().text = gameObject.name;
        InitPlayer();
    }

    /// <summary>
    /// Called once after bot is spawned. This is for intialising your bot code.
    /// </summary>
    abstract protected void InitPlayer();

    /// <summary>
    /// Called every physics cycle. This is the starting point for your bot logic. 
    /// Should always call UpdatePlayerXxx methods but the order these occur in is up to you.
    /// </summary>
    abstract protected void UpdatePlayerState();

    /// <summary>
    /// Called every physics cycle.
    /// </summary>
    void FixedUpdate()
    {
        UpdatePlayerState();
        UpdatePlayerMovement();
        UpdatePlayerRotation();
        UpdatePlayerShootingState();
    }

    /// <summary>
    /// Returns how much health the player has. Zero = corpse
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Performs a transform on the current player object to move the player based on the instruction stored in this.movePlayer 
    /// </summary>
    private void UpdatePlayerMovement()
    {
        // Move player forwards/back/left/right etc
        float diagonalMoveMultiplier = CalculateDiagonalMoveMultiplier(moveVelocity, moveVelocity);
        switch (movePlayer)
        {
            case movementTypes.Forward:
                gameObject.transform.Translate(Vector3.forward * moveVelocity * Time.fixedDeltaTime);
                break;
            case movementTypes.Back:
                gameObject.transform.Translate(Vector3.back * moveVelocity * Time.fixedDeltaTime);
                break;
            case movementTypes.Left:
                gameObject.transform.Translate(Vector3.left * moveVelocity * Time.fixedDeltaTime);
                break;
            case movementTypes.Right:
                gameObject.transform.Translate(Vector3.right * moveVelocity * Time.fixedDeltaTime);
                break;
            case movementTypes.ForwardAndLeft:
                gameObject.transform.Translate((Vector3.forward + Vector3.left) * moveVelocity * diagonalMoveMultiplier * Time.fixedDeltaTime);
                break;
            case movementTypes.ForwardAndRight:
                gameObject.transform.Translate((Vector3.forward + Vector3.right) * moveVelocity * diagonalMoveMultiplier * Time.fixedDeltaTime);
                break;
            case movementTypes.BackAndLeft:
                gameObject.transform.Translate((Vector3.back + Vector3.left) * moveVelocity * diagonalMoveMultiplier * Time.fixedDeltaTime);
                break;
            case movementTypes.BackAndRight:
                gameObject.transform.Translate((Vector3.back + Vector3.right) * moveVelocity * diagonalMoveMultiplier * Time.fixedDeltaTime);
                break;
        }
    }

    /// <summary>
    /// Performs a rotation transform on the current player object based on the rotation instruction in this.rotatePlayer
    /// </summary>
    private void UpdatePlayerRotation()
    {
        if (rotatePlayer == rotationTypes.Left)
        {
            gameObject.transform.Rotate(0, -rotationVelocity * Time.fixedDeltaTime, 0);
        }
        else if (rotatePlayer == rotationTypes.Right)
        {
            gameObject.transform.Rotate(0, rotationVelocity * Time.fixedDeltaTime, 0);
        }
    }

    /// <summary>
    /// Handles firing of weapons based on instructions from this.shootPrimaryWeapon()
    /// </summary>
    private void UpdatePlayerShootingState()
    {
        if (shootPrimaryWeapon && CanShootPrimaryWeapon())
        {
            // Clear shot timer and fire weapon
            primaryWeaponShotTimer = 0f;
            GameObject projectile = UnityEngine.Object.Instantiate(primaryWeaponProjectilePrefab); // TODO should be stored under bullets
            projectile.transform.position = primaryWeapon.transform.position;
            projectile.transform.rotation = gameObject.transform.rotation;
            projectile.GetComponent<PrimaryWeaponProjectile>().source = gameObject;
            primaryWeaponShotCounter++;
        }
        else
        {
            // Weapon not ready to fire yet or hasn't been triggered, increase timer
            primaryWeaponShotTimer += Time.fixedDeltaTime;
        }

        // Reload if magazine is empty
        if (primaryWeaponShotCounter >= primaryWeaponMagazineSize)
        {
            if (primaryWeaponReloadTimer < primaryWeaponReloadTime)
            {
                primaryWeaponReloadTimer += Time.fixedDeltaTime;
            }
            else
            {
                primaryWeaponShotCounter = 0;
                primaryWeaponReloadTimer = 0;
            }
        }
    }

    /// <summary>
    /// Can the primary weapon be fired? This will be true unless the minimum time between shots has not passed or the weapon has not finished reloading
    /// </summary>
    /// <returns>bool</returns>
    protected bool CanShootPrimaryWeapon()
    {
        return (primaryWeaponShotCounter < primaryWeaponMagazineSize && primaryWeaponShotTimer > primaryWeaponMinRefireTime);
    }

    /// <summary>
    /// If a player is moving both left and forward at the same time, they have a larger displacement so we should correct for that
    /// e.g. ((x + z) / 2) / √(x2 + z2)
    /// </summary>
    /// <param name="x">Displacement in x axis</param>
    /// <param name="z">Displacement in z axis</param>
    /// <returns>float</returns>
    private float CalculateDiagonalMoveMultiplier(float x, float z)
    {
        return ((x + z) / 2) / Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(z, 2));
    }

    /// <summary>
    /// Check for collisions with player e.g. projectile hitting player 
    /// </summary>
    /// <param name="collision">Object colliding with player</param>
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Projectile":
                GameObject attackerGameObject = collision.gameObject.GetComponent<PrimaryWeaponProjectile>().source;
                if (attackerGameObject != this.gameObject)
                {
                    // Destroy bullet and damage/kill player
                    Destroy(collision.gameObject);
                    health -= 20;
                    if(health <= 0)
                    {
                        KillPlayer(attackerGameObject);
                    }
                }
                else
                {
                   // Debug.Log("Player "+name+" shot self. No damage inflicted.");
                }
                break;
        }
    }

    /// <summary>
    /// Kill this player
    /// </summary>
    /// <param name="attacker">Player's game object that killed this player</param>
    private void KillPlayer(GameObject attacker)
    {
        gameObject.SetActive(false);
        health = 0;
        Debug.Log("Player '" + gameObject.name + "' killed by '" + attacker.name + "'");
    }
}
