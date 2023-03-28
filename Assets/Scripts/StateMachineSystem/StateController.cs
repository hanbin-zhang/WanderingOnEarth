using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum StateLabel
{
    POLLUTED,
    NORMAL,
    SAFE,
    GREENED
}

public enum BarrierSides
{
    FOWARD,
    RIGHT,
    BACKWARD,
    LEFT
}

public class StateProperty
{
    public StateLabel label { get; set; }
    public int treeNumber { get; set; }
    public int RowNum { get; set; }
    public int ColNum { get; set; }

    public List<GameObject> boundaries;

/*    public List<NaturalObject> PendingNaObjs { get; set; }
    public List<NaturalObject> EvolvingNaObjs { get; set; }
    public List<NaturalObject> EvolvedNaObjs { get; set; }*/
    public Dictionary<string, int> NaObjNums { get; set; }
    public StateProperty()
    {
        /*PendingNaObjs = new List<NaturalObject>();
        EvolvingNaObjs = new List<NaturalObject>();
        EvolvedNaObjs = new List<NaturalObject>();*/
        boundaries = new List<GameObject> { null, null, null, null };
        NaObjNums = new Dictionary<string, int>();  
    }

    public void SetState(StateLabel label)
    {
        this.label = label;
    }

    public void AddCount(string className)
    {
        if (NaObjNums.TryGetValue(className, out int value))
        {
            NaObjNums[className] = value + 1;
        }
        else
        {
            NaObjNums.Add(className, 1);
        }
    }
}

public class StateController : IEnumerable<BaseState>
{
    private EventController eventController;
    public float mapHeight, mapWidth;
    public int nRows, nColumns;
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
                        treeNumber = 0,
                        RowNum = j,
                        ColNum = i,
                    };
                }
            }
        }
        if (stateLabelMap.ContainsKey(state.StateLabel)) throw new Exception("double registered state");
        stateLabelMap[state.StateLabel] = state;        
    }

    public StateController(EventController eventController, float mapWidth = 500, float mapHeight = 500, int regionSize = 250)
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
            lock (statesProperty)
            {
                StateProperty stateProperty = GetStateProperty(msg.pos);
                GetRegionState(stateProperty).Handle(stateProperty, msg);                
            }            
        });        
        eventController.Get<OnLandPrepEvent>()?.AddListener((msg) => {
            lock (statesProperty)
            {               
                StateProperty s = GetStateProperty(msg.pos);
                GetRegionState(s).Handle(s, msg);                
                Debug.Log($"region states: {s.label}");
                Debug.Log($"property region states: {GetStateProperty(msg.pos).label}");
                Debug.Log(GetRegionState(s).GetType());
            }            
        });
    }

    public StateProperty GetStateProperty(Vector3 position)
    {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        return statesProperty[y, x];     
    }

    public BaseState GetRegionState(StateProperty property)
    {
        return stateLabelMap[property.label];
    }

    public string SerializeStatesProperty()
    {
        StringBuilder sb = new StringBuilder();

        int rowCount = StatesProperty.GetLength(0);
        int colCount = StatesProperty.GetLength(1);

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                sb.Append(((int)StatesProperty[i, j].label).ToString());
            }
        }

        return sb.ToString();
    }

    public void DeserializeStatesProperty(string data)
    {
        int rowCount = StatesProperty.GetLength(0);
        int colCount = StatesProperty.GetLength(1);

        if (data.Length != rowCount * colCount)
        {
            Debug.LogError("Invalid data length.");
            return;
        }

        int dataIndex = 0;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                int labelInt = int.Parse(data[dataIndex].ToString());
                StatesProperty[i, j].label = (StateLabel)labelInt;

                dataIndex++;
            }
        }
    }
}
