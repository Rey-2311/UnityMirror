using UnityEngine;
using Mirror;

public class CameraForCoop : NetworkBehaviour
{
    public GameObject cameraHolder; // Reference to the CameraHolder GameObject

    void Start()
    {
        // Check if this is the local player
        if (!isLocalPlayer)
        {
            // Disable the camera for non-local players
            if (cameraHolder != null)
            {
                cameraHolder.SetActive(false);
            }
        }
        else
        {
            // Enable the camera for the local player
            if (cameraHolder != null)
            {
                cameraHolder.SetActive(true);
            }
        }
    }
}

