using System.Collections;
using UnityEngine;
using Mirror;

public class RespawnMechanic : NetworkBehaviour
{
    public Vector3 checkpointPos;
    private Rigidbody rb;
    private MeshRenderer mr;
    private bool isRespawning = false;
    private MultiInventory multiInventory;

    private void Start()
    {
        checkpointPos = transform.position;
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        multiInventory = GetComponent<MultiInventory>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            Die();
        }
    }

    private void UpdateCheckpointPos()
    {
        if (isServer && isClient) // Host
        {
            checkpointPos = multiInventory.HcheckpointPos;
            Debug.Log("Host respawn");
            Debug.Log(checkpointPos);
        }
        else if (!isServer && isClient)
        {
            checkpointPos = multiInventory.CcheckpointPos;
            Debug.Log("client respawn");
            Debug.Log(checkpointPos);
        }
    }

    public void Die()
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnSequence());
        }
    }

    IEnumerator RespawnSequence()
    {
        isRespawning = true;

        // Update checkpoint position to avoid using an outdated value
        UpdateCheckpointPos();

        // Hide the sprite and disable movement
        mr.enabled = false;
        rb.linearVelocity = Vector3.zero; // Reset velocity

        // Freeze XYZ movement for 0.5 seconds
        rb.constraints = RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("check pos before respawn");
        Debug.Log(checkpointPos);
        // Respawn at the correct position
        transform.position = checkpointPos;

        mr.enabled = true;

        // Freeze X and Z movement, but allow Y for 0.5 seconds
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(0.5f);

        // Remove constraints to allow full movement again
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        isRespawning = false;
    }
}
