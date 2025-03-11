using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;             // Bullet speed
    public float maxLifetime = 2f;       // Max flight time before disappearing
    public float postCollisionTime = 1f; // Time after hitting a "Player" before disappearing

    private Rigidbody rb;
    private bool hitPlayer = false;      // Track if the bullet hit a "Player"

    void Start()
    {
        // Initialize Rigidbody and set velocity
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

        // Destroy the bullet after its max lifetime if no collision occurs
        Invoke(nameof(DestroyBullet), maxLifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject hitObject)
    {
        if (hitPlayer) return; // Avoid multiple triggers

        if (hitObject.CompareTag("Player"))
        {
            Debug.Log("Bullet hit a Player!");
            hitPlayer = true;

            // Continue flying for an additional fixed time
            Invoke(nameof(DestroyBullet), postCollisionTime);
        }
        else if (hitObject.CompareTag("DeadZone"))
        {
            Debug.Log("Bullet hit the DeadZone!");
            var respawnMechanic = hitObject.GetComponent<RespawnMechanic>();
            if (respawnMechanic != null)
            {
                respawnMechanic.Die();
            }

            DestroyBullet();
        }
        else
        {
            // Destroy bullet immediately if not hitting a player or DeadZone
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Debug.Log("Bullet destroyed");
        Destroy(gameObject);
    }
}
