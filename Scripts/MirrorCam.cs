using Mirror.Examples.AdditiveLevels;
using Mirror.Examples.Common;
using UnityEngine;

public class MirrorCam : MonoBehaviour
{
    public Transform playerCam;
    public Transform beforeGoInPortal;
    public Transform otherPortal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerOffsetFromPortal = playerCam.position - otherPortal.position;
        transform.position = beforeGoInPortal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(beforeGoInPortal.rotation, otherPortal.rotation);
        
        Quaternion portalRotationalDiff = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCamDirection = portalRotationalDiff * playerCam.forward;
        transform.rotation = Quaternion.LookRotation(newCamDirection, Vector3.up);
    
    }
}
