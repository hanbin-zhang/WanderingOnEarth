using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [HideInInspector] public List<CollectableObject> collectables; // An array of all the collectables in the scene

    private List<CollectableObject> activeCollectables; // A list of the collectables within the player's region

    public GameObject CollectMessagePanel;

    private void Start()
    {

        // Initialize the list of active collectables
        activeCollectables = new List<CollectableObject>();
        collectables = new List<CollectableObject>();
        CollectableObject[] collectableArray = FindObjectsOfType<CollectableObject>();
        for (int i = 0; i < collectableArray.Length; i++)
        {
            collectables.Add(collectableArray[i]);
        }


    }

    private void Update()
    {
        CheckCollectable();
    }

    public void CheckCollectable(bool keyPressed=false)
    {
        CollectMessagePanel.SetActive(false);
        if (collectables == null)
        {
            /*collectables = FindObjectsOfType<CollectableObject>();*/
        }
        else
        {
            // Clear the list of active collectables
            activeCollectables.Clear();


            // Loop through all the collectables in the scene
            for (int i = 0; i < collectables.Count; i++)
            {
                CollectableObject collectable = collectables[i];
                // Check if the collectable is within the player's region
                if (IsCollectableInRegion(transform.position, collectable.transform.position))
                {
                    // Add the collectable to the list of active collectables
                    activeCollectables.Add(collectable);
                }

            }
            // Loop through the active collectables and call the Update method
            for (int i = 0; i < activeCollectables.Count; i++)
            {
                CollectableObject collectable = activeCollectables[i];
                if (collectable.CheckCollect(transform.position, CollectMessagePanel, keyPressed))
                {
                    collectables.Remove(collectable);
                    Destroy(collectable.gameObject);
                }
            }
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
