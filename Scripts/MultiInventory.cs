using UnityEngine;
using Mirror;
using Mirror.Examples.RigidbodyBenchmark;
using DG.Tweening;
using NUnit.Framework.Internal;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;
public class MultiInventory : NetworkBehaviour
{
    RespawnMechanic respawnMechanic;
    public Vector3 HcheckpointPos = new Vector3(0,0,0);
    public Vector3 CcheckpointPos = new Vector3(0, 0, 0);
    public int HInventory = 0;
    public int CInventory = 0;
    PowerUp pu;
    private string thisObjectName;
    private int randomNumber;
    private void Awake()
    {
        respawnMechanic = GameObject.FindGameObjectWithTag("Player").GetComponent<RespawnMechanic>();
        pu = GameObject.FindGameObjectWithTag("PowerUp").GetComponent<PowerUp>();
        thisObjectName = gameObject.name;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // The name of the object that owns this script
        if(collision.CompareTag("Checkpoint"))
        {
            if (thisObjectName == "Player") //if player is server and touches "Checkpoint"
            {
                Vector3 centerPosition = collision.bounds.center;
                HcheckpointPos = new Vector3(centerPosition.x, centerPosition.y + 2f, centerPosition.z);
                
            }
            else if (thisObjectName != "Player") // same but for client
            {
                Vector3 centerPosition = collision.bounds.center;
                CcheckpointPos = new Vector3(centerPosition.x, centerPosition.y + 2f, centerPosition.z);
                
            }
        }

        if(collision.CompareTag("PowerUp"))
        {
            randomNumber = UnityEngine.Random.Range(1, 101);
            if (thisObjectName == "Player")
            {
                Debug.Log("server powerUP");
                if (randomNumber <= 20)
                    pu.wind();
                else if (randomNumber <= 40)
                    pu.turret();
                else if (randomNumber <= 60)
                    pu.illusionPortal();
                else if (randomNumber <= 80)
                    pu.audioGun();
                else if (randomNumber <= 90)
                    pu.audioCamera();
                else
                    pu.ignoreDeath();
            }
            else if(thisObjectName != "Player") //suspect add check isClient
            {
                Debug.Log("Client powerUP");
                if (randomNumber <= 20)
                    pu.wind();
                else if (randomNumber <= 40)
                    pu.turret();
                else if (randomNumber <= 60)
                    pu.illusionPortal();
                else if (randomNumber <= 80)
                    pu.audioGun();
                else if (randomNumber <= 90)
                    pu.audioCamera();
                else
                    pu.ignoreDeath();
            }
        }
    }
}
//neu powerup k lam dc thi sync