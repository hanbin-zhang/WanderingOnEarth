using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum StateLabel
{
    POLLUTED,
    NORMAL,
    GREENED
}

public class StateController : IEnumerable<BaseState>
{
    private EventController eventController;
    public float mapHeight, mapWidth;
    private int nRows, nColumns;
    public int regionSize;
    private StateLabel[,] regionStates;

    public Dictionary<StateLabel, BaseState> stateLabelMap = new Dictionary<StateLabel, BaseState>();
    public IEnumerator<BaseState> GetEnumerator() => stateLabelMap.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => stateLabelMap.Values.GetEnumerator();
    public void Add(BaseState state)
    {
        if (!stateLabelMap.Any())
        {
            for (int j = 0; j < nRows; j++)
            {
                for (int i = 0; i < nColumns; i++)
                {
                    regionStates[j, i] = state.StateLabel;
                }
            }
        }
        stateLabelMap[state.StateLabel] = state;
    }

    public StateController(EventController eventController, float mapWidth = 1000, float mapHeight = 1000, int regionSize = 500)
    {
        this.eventController = eventController;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.regionSize = regionSize;
        this.nRows = (int)(mapHeight / regionSize);
        this.nColumns = (int)(mapWidth / regionSize);

        regionStates = new StateLabel[nRows, nColumns];
        BindEvents();
    }

    public void BindEvents()
    {
        eventController.Get<OnPlantEvent>()?.AddListener((msg) => {
            
            
            getRegionState(msg.pos).Handle(this, msg);
            string log = "";
            foreach (StateLabel label in regionStates)
            {
                log += label + " ";
            }
            Debug.Log($"region states: {log}");
        });
        eventController.Get<OnWaterEvent>()?.AddListener((msg) => {
            //currentState.Handle(this, msg);
        });
    }

    public BaseState getRegionState(Vector3 position)
    {
        return stateLabelMap[getRegionStateLabel(position)];
    }

    public StateLabel getRegionStateLabel(Vector3 position)
    {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        return regionStates[y, x];
    }

    public void setRegionState(Vector3 position, BaseState state)
    {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        regionStates[y, x] = state.StateLabel;
    }

    public void setRegionStateLabel(Vector3 position, StateLabel state)
    {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        regionStates[y, x] = state;
    }
}


