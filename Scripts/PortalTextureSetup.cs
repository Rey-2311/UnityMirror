using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera MirrorCam;
    public Material cameraMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MirrorCam.targetTexture != null)
        {
            MirrorCam.targetTexture.Release();
        }
        MirrorCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMat.mainTexture = MirrorCam.targetTexture;

    }

    // Update is called once per frame
    void Update()
    {
        
    }  
}
