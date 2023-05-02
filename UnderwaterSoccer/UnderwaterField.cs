using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterField : MonoBehaviour
{
    // The width of the field
    public float width = 20f;

    // The length of the field
    public float length = 30f;

    // The number of coins to spawn
    public int numCoins = 10;

    // The coin prefab to spawn
    public GameObject coinPrefab;

    void Start()
    {
        // Create the coins
        for (int i = 0; i < numCoins; i++)
        {
            Vector3 position = new Vector3(Random.Range(-width/2, width/2), 0, Random.Range(-length/2, length/2));
            Instantiate(coinPrefab, position, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the outline of the field in the scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 1, length));
    }
}