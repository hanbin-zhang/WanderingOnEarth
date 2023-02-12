using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public CollectableObject[] collectables; // An array of all the collectables in the scene

    private List<CollectableObject> activeCollectables; // A list of the collectables within the player's region

    private void Start()
    {
        // Initialize the list of active collectables
        activeCollectables = new List<CollectableObject>();
    }

    private void Update()
    {
        // Clear the list of active collectables
        activeCollectables.Clear();

        // Get the player's position
        Vector3 playerPos = this.transform.position;

        // Loop through all the collectables in the scene
        for (int i = 0; i < collectables.Length; i++)
        {
            CollectableObject collectable = collectables[i];

            // Check if the collectable is within the player's region
            if (IsCollectableInRegion(playerPos, collectable.transform.position))
            {
                // Add the collectable to the list of active collectables
                activeCollectables.Add(collectable);
            }
        }

        // Loop through the active collectables and call the Update method
        for (int i = 0; i < activeCollectables.Count; i++)
        {
            CollectableObject collectable = activeCollectables[i];
            collectable.CheckCollect(playerPos);
        }
    }

    private bool IsCollectableInRegion(Vector3 playerPos, Vector3 collectablePos)
    {
        // Calculate the distance between the player and the collectable
        float distance = Vector3.Distance(playerPos, collectablePos);

        // Return true if the distance is less than a certain value
        return distance < 10.0f;
    }
}
