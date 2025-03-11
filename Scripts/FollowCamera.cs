using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 localOffset = new Vector3(0, 0, 1);  // Offset in local space

    void Start()
    {
        // Make the mirror a child of the camera to inherit its movement and rotation
        transform.SetParent(cameraTransform);

        // Set the initial local position to be in front of the camera
        transform.localPosition = localOffset;

        // Rotate the mirror to face the correct direction
        transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
    }
}
