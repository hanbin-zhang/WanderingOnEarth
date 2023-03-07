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

    public List<NaturalObject> PendingNaObjs { get; set; }
    public List<NaturalObject> EvolvingNaObjs { get; set; }
    public List<NaturalObject> EvolvedNaObjs { get; set; }
    public StateProperty()
    {
        PendingNaObjs = new List<NaturalObject>();
        EvolvingNaObjs = new List<NaturalObject>();
        EvolvedNaObjs = new List<NaturalObject>();
    }

    public Dictionary<string, int> NaObjNumbers()
    {
        Dictionary<string, int> result = new();
        List < NaturalObject > mergedList = new List < NaturalObject >();
        mergedList.AddRange(EvolvedNaObjs);
        mergedList.AddRange(EvolvingNaObjs);
        mergedList.AddRange(PendingNaObjs);
        foreach (NaturalObject n in mergedList)
        {
            if (result.ContainsKey(n.GetDerivedClassName())) {
                result[n.GetDerivedClassName()]++;
            } else result[n.GetDerivedClassName()]=1;
        }

        return result;
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
        /*Action<OnPlantEvent.OnPlantMessage> onPlantAction = (msg) => {
            getRegionState(msg.pos).Handle(this, msg);
            string log = "";
            foreach (StateLabel label in regionStates)
            {
                log += label + " ";
            }
            Debug.Log($"region states: {log}");
        };
        eventController.Get<OnPlantEvent>()?.AddListener(onPlantAction);
        // 实现接口方式 listener
        eventController.Get<OnPlantEvent>()?.AddListener(new StateOnPlantEventListener(this));*/
        // 实现lambda方式 action

        eventController.Get<OnPlantEvent>()?.AddListener((msg) => {
            lock (statesProperty)
            {
                StateProperty stateProperty = GetStateProperty(msg.pos);
                StateLabel newState = GetRegionState(stateProperty.label).Handle(stateProperty, msg);
                stateProperty.label = newState;
            }
            /*string log = "";
            foreach (StateProperty s in statesProperty)
            {
                log += s.label + " ";
            }
            Debug.Log($"region states: {log}");*/
        });
        eventController.Get<OnWaterEvent>()?.AddListener((msg) => {
            lock (statesProperty)
            {

            }
            /*foreach (StateProperty s in statesProperty)
            {
                StateLabel newState = GetRegionState(s.label).Handle(s, msg);
                s.label = newState;
            }*/
        });
        eventController.Get<OnLandPrepEvent>()?.AddListener((msg) => {
            lock (statesProperty)
            {
                StateProperty s = GetStateProperty(msg.pos);
                StateLabel newState = GetRegionState(s.label).Handle(s, msg);
                s.label = newState;
                Debug.Log($"region states: {newState}");
                Debug.Log($"property region states: {GetStateProperty(msg.pos).label}");
            }            
        });
    }

    public StateProperty GetStateProperty(Vector3 position)
    {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        return statesProperty[y, x];     
    }

    public BaseState GetRegionState(StateLabel stateLabel)
    {
        return stateLabelMap[stateLabel];
    }
}


public class StateOnPlantEventListener : OnPlantEvent.OnPlantListener
{
    private StateController stateController;
    private StateProperty stateProperty;

    public StateOnPlantEventListener(StateController stateController, StateProperty stateProperty)
    {
        this.stateController = stateController;
        this.stateProperty = stateProperty;
    }

    public void OnEvent(OnPlantEvent.OnPlantMessage msg)
    {
        StateLabel newState = stateController.GetRegionState(stateProperty.label).Handle(stateProperty, msg);
    }
}