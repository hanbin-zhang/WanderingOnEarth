using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    
    protected StateLabel stateLabel;
    public StateLabel StateLabel => stateLabel;


    public abstract StateLabel Handle(StateProperty stateProperty, BaseMessage msg);
}
