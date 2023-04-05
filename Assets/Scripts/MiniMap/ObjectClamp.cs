using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClamp : MonoBehaviour
{
    public GameObject playerIconPrefab; // prefab for the player icon
    public int maxPlayerIcons = 10; // maximum number of player icons to pool
    public string playerTag = "Player"; // tag of the player GameObjects
    public float MinimapSize;
    public float outRange;

    private List<Transform> otherPlayerTransforms = new(); // list of other player transforms
    private List<GameObject> playerIcons = new(); // list of player icon GameObjects
    private List<GameObject> pooledIcons = new(); // list of pooled player icon GameObjects


    void Start()
    {
        // create pool of player icons
        for (int i = 0; i < maxPlayerIcons; i++)
        {
            GameObject playerIcon = Instantiate(playerIconPrefab, transform);
            playerIcon.SetActive(false);
            pooledIcons.Add(playerIcon);
        }

        // set initial positions and rotations of player icons
        for (int i = 0; i < pooledIcons.Count; i++)
        {
            pooledIcons[i].transform.localPosition = Vector3.one;
            pooledIcons[i].transform.localRotation = Quaternion.identity;
        }
    }

    void LateUpdate()
    {
        // find all other players
        GameObject[] otherPlayers = GameObjectTracker.playerObjects.ToArray();

        // check for added or removed players
        List<Transform> newOtherPlayerTransforms = new();

        foreach (GameObject gameObject in playerIcons)
        {
            gameObject.SetActive(false);
            pooledIcons.Add(gameObject);
        }

        playerIcons.Clear();

        foreach (GameObject otherPlayer in otherPlayers)
        {
            if (DistanceOnXZPlane(otherPlayer.transform.position,
                transform.position) >= outRange)
            {   
                newOtherPlayerTransforms.Add(otherPlayer.transform);
                // activate pooled player icon and add to list
                if (pooledIcons.Count > 0)
                {
                    GameObject playerIcon = pooledIcons[pooledIcons.Count - 1];
                    pooledIcons.RemoveAt(pooledIcons.Count - 1);
                    playerIcon.SetActive(true);
                    playerIcons.Add(playerIcon);
                }
            }
        }
        otherPlayerTransforms = newOtherPlayerTransforms;


        // update player icon positions and rotations
        for (int i = 0; i < otherPlayerTransforms.Count; i++)
        {
            Transform otherPlayerTransform = otherPlayerTransforms[i];
            GameObject playerIcon = playerIcons[i];

            // calculate direction to other player
            Quaternion playerRotation = Quaternion.Inverse(transform.rotation);
            Vector3 direction = otherPlayerTransform.position - transform.position;
            direction.y = 0;
            float distance = direction.magnitude;
            direction = playerRotation * direction;
            direction.Normalize();

            // update player icon position and rotation
            playerIcon.transform.localPosition = GetTargetLocation(direction, MinimapSize);
            // set icon position based on direction
            Vector3 targetPoint = otherPlayerTransform.position;
            targetPoint.y = playerIcon.transform.position.y;
            playerIcon.transform.LookAt(targetPoint); // point icon towards other player
            Vector3 rotation = new Vector3(0f, 180f, 0f); // Create a Vector3 with the rotation values we want (180 degrees around y-axis)
            playerIcon.transform.Rotate(rotation);

        }
    }

    public float DistanceOnXZPlane(Vector3 point1, Vector3 point2)
    {
        // Set the y-coordinates of both points to zero to ignore the y-axis
        point1.y = 0f;
        point2.y = 0f;

        // Calculate the distance between the two points on the x-z plane
        float distance = Vector3.Distance(point1, point2);

        return distance;
    }

    public Vector3 GetTargetLocation(Vector3 direction, float distance, Vector3 origin = default)
    {
        // Normalize the direction vector to get a unit vector
        Vector3 unitDirection = direction.normalized;

        Vector3 targetLocation = origin + unitDirection * distance;

        targetLocation.y = 20f;

        return targetLocation;
    }
}
