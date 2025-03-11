using UnityEngine;
using Mirror;
using Mirror.Examples.RigidbodyBenchmark;
using UnityEngine.Rendering.Universal;
public class PowerUp : NetworkBehaviour
{
    private MultiInventory multiInventory;
    private float ignoreDeathTime = 0f;
    private bool isIgnoreDeath = false;
    public GameObject machineGunPrefab;
    private void Start()
    {
        multiInventory = GetComponent<MultiInventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }
 
    void PickUp()
    {
        Destroy(gameObject);
    }

    /////////////////////////////////////////////////////////////////////////////            debuff
  
    public void wind()
    {
        //addforce to x
        Debug.Log("wind");
    }

    public void turret ()
    {
        Debug.Log("turret");
    }

    public void illusionPortal ()
    {
        Debug.Log("illusionPortal");
    }

    public void audioGun()
    {
        Debug.Log("audioGun");
    }

    public void audioCamera()//test idea
    {
        Debug.Log("audioCamera");
    }

    /////////////////////////////////////////////////////////////////////////////            buff
    public void ignoreDeath() // need optimzed
    {
        ignoreDeathTime = 10f;
        isIgnoreDeath = true;
        Debug.Log("ignoreDeath");
    }
    /////////////////////////////////////////////////////////////////////////////            function

    //private void Update()
    //{
    //    if (isIgnoreDeath)
    //    ignoreDeathTime -= 1f;
    //    if (ignoreDeathTime ==  0)
    //    {
    //        isIgnoreDeath = false;
    //    }
    //}
}
