using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;


public enum StateLabel
{
    POLLUTED,
    NORMAL,
    SAFE,
    GREENED
}


public class StateProperty
{
    public int index { get; set; }
    public StateLabel state { get; set; }
    public List<GameObject> boundaries = new List<GameObject> { null, null, null, null };

    public void SetState(StateLabel newState) {
        if (state != newState) {
            StateLabel oldState = state;
            state = newState;
            Manager.EventController.Get<OnStateChangeEvent>()?.Notify(oldState, newState, this);
        }
    }

    public override string ToString() {
        return $"[StateProperty] {index}: {state}";
    }
}

public class StateController : IEnumerable<BaseState>
{
    private EventController eventController;
    public float mapHeight, mapWidth;
    public int nRows, nColumns;
    public int regionSize;
    private List<StateProperty> statesProperty;
    public List<StateProperty> StatesProperty => statesProperty;
    public Dictionary<StateLabel, BaseState> stateLabelMap = new Dictionary<StateLabel, BaseState>();
    public IEnumerator<BaseState> GetEnumerator() => stateLabelMap.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => stateLabelMap.Values.GetEnumerator();
    public void Add(BaseState state) {
        if (!stateLabelMap.Any()) {      
            for (int i = 0; i < nRows * nColumns; i++) {
                statesProperty.Add(new StateProperty() {
                    index = i,
                    state = state.StateLabel,
                });
            }
        }
        if (stateLabelMap.ContainsKey(state.StateLabel)) throw new Exception("double registered state");
        stateLabelMap[state.StateLabel] = state;        
    }

    public StateController(EventController eventController, float mapWidth=500, float mapHeight=500, int regionSize=250) {
        this.eventController = eventController;
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.regionSize = regionSize;
        this.nRows = (int)(mapHeight / regionSize);
        this.nColumns = (int)(mapWidth / regionSize);
        this.statesProperty = new List<StateProperty>(nRows * nColumns);
        PhotonPeer.RegisterType(typeof(StateProperty), (byte)'S', SerializeStateProperty, DeserializeStateProperty);
        BindEvents();
    }

    public void BindEvents() {       
        eventController.Get<OnPlantEvent>()?.AddListener((msg) => {
            StateProperty stateProperty = GetRegionalStateProperty(msg.pos);
            GetRegionalState(stateProperty).Handle(stateProperty, msg);                      
        });       

        eventController.Get<OnLandPrepEvent>()?.AddListener((msg) => {
            StateProperty stateProperty = GetRegionalStateProperty(msg.pos);
            GetRegionalState(stateProperty).Handle(GetRegionalStateProperty(msg.pos), msg);
            
        });

    }

    public StateProperty GetRegionalStateProperty(int index) {
        return statesProperty[index];
    }

    public StateProperty GetRegionalStateProperty(Vector3 position) {
        return GetRegionalStateProperty(GetRegionalIndex(position));
    }
    
    public int GetRegionalIndex(Vector3 position) {
        int x = (int)Mathf.Clamp(position.x / regionSize, 0, nColumns - 1);
        int y = (int)Mathf.Clamp(position.z / regionSize, 0, nRows - 1);
        return y * nColumns + x;
    }

    public BaseState GetRegionalState(StateProperty property) {
        return stateLabelMap[property.state];
    }

    public BaseState GetRegionalState(int index) {
         return GetRegionalState(GetRegionalStateProperty(index));
    }

    public BaseState GetRegionalState(Vector3 pos) {
        return GetRegionalState(GetRegionalStateProperty(pos));
    }

    public override string ToString() {
        return $"[StateController] Count: {statesProperty.Count}\n{statesProperty.Select((x) => $"{x.ToString()}\n").Aggregate((t, x) => t += x)}";
    }

    public static readonly byte[] buffer = new byte[2 * 5];
    public short SerializeStateProperty(StreamBuffer outStream, object customObject) {
        StateProperty stateProperty = (StateProperty)customObject;
        lock (buffer) {
            byte[] bytes = buffer;
            int index = 0;
            Protocol.Serialize((int)stateProperty.state, bytes, ref index);
            Protocol.Serialize(stateProperty.index, bytes, ref index);
            outStream.Write(bytes, 0, 2 * 5);
        }
        return 2 * 5;
    }

    public object DeserializeStateProperty(StreamBuffer inStream, short length) {
        int stateLabel = 0;
        int stateIndex = 0;
        lock (buffer) {
            inStream.Read(buffer, 0, 2 * 5);
            int index = 0;
            Protocol.Deserialize(out stateLabel, buffer, ref index);
            Protocol.Deserialize(out stateIndex, buffer, ref index);
        }
        return new StateProperty() {
            state = (StateLabel)stateLabel,
            index = stateIndex,
        };
    } 

}
