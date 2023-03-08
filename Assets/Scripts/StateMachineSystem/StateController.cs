using System;
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

public class StateProperty
{
    public StateLabel label { get; set; }
    public int greenValue { get; set; }

    public void SetState(StateLabel label) 
    {
        this.label = label;
    }
}

public class StateController : IEnumerable<BaseState>
{
    private EventController eventController;
    public float mapHeight, mapWidth;
    private int nRows, nColumns;
    public int regionSize;
    
    private StateProperty[,] statesProperty;
    public StateProperty[,] StatesProperty => statesProperty;

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
                    statesProperty[j, i] = new StateProperty()
                    {
                        label = state.StateLabel,
                        greenValue = 0
                    };
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

        statesProperty = new StateProperty[nRows, nColumns];
        BindEvents();
    }

    public void BindEvents()
    {

        eventController.Get<OnPlantEvent>()?.AddListener((msg) => {
            StateProperty stateProperty = GetStateProperty(msg.pos);
            GetRegionState(stateProperty).Handle(stateProperty, msg);

            string log = "";
            foreach (StateProperty s in statesProperty)
            {
                log += s.label + " ";
            }
            Debug.Log($"region states: {log}");
        });
        eventController.Get<OnWaterEvent>()?.AddListener((msg) => {
            foreach (StateProperty s in statesProperty)
            {
                GetRegionState(s).Handle(s, msg);
            }
        });        
    }

    public StateProperty GetStateProperty(Vector3 position)
    {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        return statesProperty[y, x];     
    }

    public BaseState GetRegionState(StateProperty stateProperty)
    {
        return stateLabelMap[stateProperty.label];
    }
}

