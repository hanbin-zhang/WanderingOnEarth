using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public int collectableID;
    public float collectDistance;
    public string collectMessage = "Press 'C' to collect";

    public bool CheckCollect(Vector3 playerPosition)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= collectDistance)
        {
            // Show collect message
            ShowCollectMessage();

            // Check if player presses 'P' to collect
            if (Input.GetKeyDown(KeyCode.C))
            {
                Collect();
                return true;    
            }
        }
        return false;
    }

    private void ShowCollectMessage()
    {
        Debug.Log(collectMessage);
    }

    private void Collect()
    {
        // Add collect logic here
        Debug.Log("Collectable collected!");
    }
}
