using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a projectile from the primary weapon and handles propelling it
/// </summary>
public class PrimaryWeaponProjectile : MonoBehaviour {

    /// <summary>
    /// How long the projectile should remain alive before it disappears (assuming it doesn't hit something in the meantime)
    /// </summary>
    private const float timeToLive = 1.5f;

    /// <summary>
    /// Primary weapon projectile velocity.
    /// </summary>
    protected const float projectileVelocity = 7f;

    /// <summary>
    /// Current count of how long the projectile has been alive for
    /// </summary>
    private float timeAlive = 0f;

    /// <summary>
    /// Source of projectile
    /// </summary>
    [HideInInspector] public GameObject source;

    /// <summary>
    /// Called once per physics cycle. Update bullet status
    /// </summary>
    void FixedUpdate () {
	    if(timeAlive > timeToLive)
        {
            // Projectile has been alive for too long, destroy it
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            // Projectile is still in motion, update it's position
            gameObject.transform.Translate(Vector3.forward * projectileVelocity * Time.fixedDeltaTime);
            timeAlive += Time.fixedDeltaTime;
            if(timeAlive > ((1 / projectileVelocity) * 0.75f))
            {
                // Only enable the projectile collider once it's left the gun barrel
                SphereCollider collider = GetComponent<SphereCollider>();
                collider.enabled = true;
            }
        }
	}
}
