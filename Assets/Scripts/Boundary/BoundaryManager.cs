using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoundaryManager : MonoBehaviour
{
    public GameObject Boundary;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { this.enabled = false; return; }
        else
        {
            Manager.EventController.Get<OnStateChangeEvent>()?.AddListener((msg) =>
            {

                StateProperty s = Manager.StateController.GetStateProperty(msg.pos);
                if (msg.StateLabel == StateLabel.SAFE) InstantiateBoundary(s);
            });
        }
    }

    public void InstantiateBoundary(StateProperty stateProperty)
    {   
        if (!PhotonNetwork.IsMasterClient) { return; }
        int regionSize = Manager.StateController.regionSize;
        Vector3 originPosition = new Vector3(stateProperty.ColNum * regionSize, 0f, stateProperty.RowNum * regionSize);

        float halfSize = regionSize / 2f;
        Quaternion rotation = Quaternion.Euler(0f, 90f, 0f);

        Vector3[] positions = new Vector3[] {
            new Vector3(halfSize, 0f, regionSize) + originPosition,
            new Vector3(regionSize, 0f, halfSize) + originPosition,
            new Vector3(halfSize, 0f, 0f) + originPosition,
            new Vector3(0f, 0f, halfSize) + originPosition
        };

        Quaternion[] rotations = new Quaternion[] {
            Quaternion.identity,
            rotation,
            Quaternion.identity,
            rotation
        };

        for (int i = 0; i < positions.Length; i++)
        {
            switch ((BarrierSides)i)
            {
                case BarrierSides.FOWARD:
                    if (originPosition.z + 2 * regionSize > Manager.StateController.mapHeight)
                    {
                        stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                    }
                    else
                    {
                        StateProperty adjacentProperty = Manager.StateController.GetStateProperty(originPosition + new Vector3(0f, 0f, 2 * regionSize));
                        if (adjacentProperty.boundaries[i + 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i + 2].SetActive(false);
                            // Destroy the current wall
                            PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            stateProperty.boundaries[i] = null;
                            continue; // Skip to the next iteration of the loop
                        }
                        else
                        {
                            stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                        }
                    }
                    continue;

                case BarrierSides.RIGHT:
                    if (originPosition.x + 2 * regionSize > Manager.StateController.mapWidth)
                    {
                        stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                    }
                    else
                    {
                        StateProperty adjacentProperty = Manager.StateController.GetStateProperty(originPosition + new Vector3(2 * regionSize, 0f, 0f));
                        if (adjacentProperty.boundaries[i + 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i + 2].SetActive(false);
                            // Destroy the current wall
                            PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            stateProperty.boundaries[i] = null;
                            continue; // Skip to the next iteration of the loop
                        }
                        else
                        {
                            stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                        }
                    }
                    continue;
                case BarrierSides.BACKWARD:
            
                    if (originPosition.z - regionSize < 0)
                    {
                        stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                    }
                    else
                    {
                        StateProperty adjacentProperty = Manager.StateController.GetStateProperty(originPosition - new Vector3(0f, 0f, regionSize));
                        Debug.Log($"new posi{originPosition - new Vector3(regionSize, 0f, 0f)}");
                        if (adjacentProperty.boundaries[i - 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i - 2].SetActive(false);
                            // Destroy the current wall
                            PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            stateProperty.boundaries[i] = null;
                            continue; // Skip to the next iteration of the loop
                        }
                        else
                        {
                            stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                        }
                    }
                    continue;
                case BarrierSides.LEFT:
                    if (originPosition.x - regionSize < 0)
                    {
                        stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                    }
                    else
                    {
                        StateProperty adjacentProperty = Manager.StateController.GetStateProperty(originPosition - new Vector3(regionSize, 0f, 0f));
                        if (adjacentProperty.boundaries[i - 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i - 2].SetActive(false);
                            // Destroy the current wall
                            PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            stateProperty.boundaries[i] = null;
                            continue; // Skip to the next iteration of the loop
                        }
                        else
                        {
                            stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                        }
                    }
                    break;
                default:
                    stateProperty.boundaries[i] = PhotonNetwork.Instantiate(Boundary.name, positions[i], rotations[i]);
                    continue;
            }
        }
    }
}
