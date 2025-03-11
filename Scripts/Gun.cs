using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab to spawn
    public Transform firePoint;    // Position to spawn bullets from
    public float fireRate = 0.5f;  // Time interval between each shot

    void Start()
    {
        // Start firing bullets repeatedly when the game begins
        StartCoroutine(FireContinuously());
    }

    IEnumerator FireContinuously()
    {
        while (true) // Infinite loop to keep firing
        {
            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Debug.Log("Bullet fired!");
            }
            else
            {
                Debug.LogWarning("Bullet prefab or fire point is not assigned!");
            }

            yield return new WaitForSeconds(fireRate); // Wait before firing the next bullet
        }
    }
}
