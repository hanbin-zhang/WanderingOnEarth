using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public int collectableID;
    public float collectDistance;
    public string collectDesc;


    public bool CheckCollect(Vector3 playerPosition, GameObject messagePanel)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= collectDistance)
        {
            // Show collect message
            ShowCollectMessage(messagePanel);

            // Check if player presses 'P' to collect
            if (Input.GetKeyDown(KeyCode.C))
            {
                Collect();
                return true;    
            }
        }
        return false;
    }

    private void ShowCollectMessage(GameObject messagePanel)
    {
        messagePanel.SetActive(true);
    }

    private void Collect()
    {
        GameObjectTracker.collected.Add(this.collectableID, collectDesc);
    }
}
