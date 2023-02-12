using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public int collectableID;
    public float collectDistance = 2.0f;
    public string collectMessage = "Press 'P' to collect";
    public Vector3 collectPosition;

    public void Update()
    {
        CheckCollect(collectPosition);
    }

    public void CheckCollect(Vector3 playerPosition)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= collectDistance)
        {
            // Show collect message
            ShowCollectMessage();

            // Check if player presses 'P' to collect
            if (Input.GetKeyDown(KeyCode.P))
            {
                Collect();
            }
        }
    }

    private void ShowCollectMessage()
    {
        Rect rect = new(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50);
        GUI.Label(rect, collectMessage);
    }

    private void Collect()
    {
        // Add collect logic here
        Debug.Log("Collectable collected!");

        // Destroy the collectable object
        Destroy(gameObject);
    }
}
