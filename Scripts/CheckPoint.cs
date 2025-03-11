using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    RespawnMechanic respawnMechanic;
    private void Awake()
    {
        respawnMechanic = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnMechanic>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("touch");
        }
    }

}
