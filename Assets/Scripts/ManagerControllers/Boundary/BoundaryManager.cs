using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public enum BarrierSides
{
    FOWARD = 0,
    RIGHT = 1,
    BACKWARD = 2,
    LEFT = 3,
}

public class BoundaryManager : MonoBehaviour
{
    public GameObject Boundary;

    private bool hasAssigned;

    private void Start() {
        Init();
    }

    public void Init() {
        // Only the master client manage the boundaries
        // and use PhotonNetwork.Instantiate() to sync them
        if (PhotonNetwork.IsMasterClient && !hasAssigned) {
            Manager.EventController.Get<OnStateChangeEvent>()?.AddListener((msg) => {
                if (msg.stateAfter == StateLabel.SAFE) {
                    InstantiateBoundary(msg.stateProperty);
                }
            });
            hasAssigned = true;
        }
    }


    public void InstantiateBoundary(StateProperty stateProperty)
    {   
        int nColumns = Manager.StateController.nColumns;
        int y = stateProperty.index / nColumns, x = stateProperty.index % nColumns;

        if (!PhotonNetwork.IsMasterClient) { return; }
        int regionSize = Manager.StateController.regionSize;
        Vector3 originPosition = new Vector3(x * regionSize, 0f, y * regionSize);

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
                        StateProperty adjacentProperty = Manager.StateController.GetRegionalStateProperty(originPosition + new Vector3(0f, 0f, 2 * regionSize));
                        if (adjacentProperty.boundaries[i + 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i + 2].SetActive(false);
                            // Destroy the current wall
                            if (stateProperty.boundaries[i] != null)
                            {
                                PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            }
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
                        StateProperty adjacentProperty = Manager.StateController.GetRegionalStateProperty(originPosition + new Vector3(2 * regionSize, 0f, 0f));
                        if (adjacentProperty.boundaries[i + 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i + 2].SetActive(false);
                            // Destroy the current wall
                            if (stateProperty.boundaries[i] != null)
                            {
                                PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            }                            
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
                        StateProperty adjacentProperty = Manager.StateController.GetRegionalStateProperty(originPosition - new Vector3(0f, 0f, regionSize));
                        Debug.Log($"new posi{originPosition - new Vector3(regionSize, 0f, 0f)}");
                        if (adjacentProperty.boundaries[i - 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i - 2].SetActive(false);
                            // Destroy the current wall
                            if (stateProperty.boundaries[i] != null)
                            {
                                PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            }
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
                        StateProperty adjacentProperty = Manager.StateController.GetRegionalStateProperty(originPosition - new Vector3(regionSize, 0f, 0f));
                        if (adjacentProperty.boundaries[i - 2] is not null)
                        {
                            // Disable the corresponding wall in the adjacent region
                            adjacentProperty.boundaries[i - 2].SetActive(false);
                            // Destroy the current wall
                            if (stateProperty.boundaries[i] != null)
                            {
                                PhotonNetwork.Destroy(stateProperty.boundaries[i]);
                            }
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

    /*
    private bool hasNeighour(int x, int y) {
        if (y < 0 || x < 0) return false;
        if (y >= Manager.StateController.nRows || x >= Manager.StateController.nColumns) return false; 
        return true;
    }

    private bool isDangerous(int x, int y) {
        int nColumns = Manager.StateController.nColumns;
        return Manager.StateController.StatesProperty[y * nColumns + x].state == StateLabel.POLLUTED 
            || Manager.StateController.StatesProperty[y * nColumns + x].state == StateLabel.NORMAL;

    }

    private void InstantiateBoundary() {
        List<StateProperty> regionalStates = Manager.StateController.StatesProperty;
        int nColumns = Manager.StateController.nColumns;
        for (int k = 0; k < regionalStates.Count; k++) {
            if (regionalStates[k].state == StateLabel.SAFE) {
                int y = k / nColumns, x = k % nColumns;
                (int, int)[] pos = new (int, int)[] { 
                    (x, y - 1), // FORWARD
                    (x + 1, y), // RIGHT
                    (x, y + 1), // BACKWARD
                    (x - 1, y), // LEFT
                 };
                for (int side = 0; side < pos.Length; side++) {
                    int i = pos[side].Item1, j = pos[side].Item2;
                    if (!hasNeighour(i, j)) {
                        ShowBoundary(side, i, j);
                        continue;
                    }
                    if (isDangerous(i, j)) {
                        ShowBoundary(side, i, j);
                    }
                }
            }
        }
    }


    private void ShowBoundary(int side, int i, int j) {
        
        int regionSize = Manager.StateController.regionSize;
        Vector3 originPosition = new Vector3(i * regionSize, 0f, j * regionSize);

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
        PhotonNetwork.Instantiate(Boundary.name, positions[side], rotations[side]);  
    }
*/




}
