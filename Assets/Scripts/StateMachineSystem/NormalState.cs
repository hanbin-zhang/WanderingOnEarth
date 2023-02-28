using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class NormalState : BaseState
{
    public NormalState() => stateLabel = StateLabel.NORMAL;

    public override StateLabel Handle(StateProperty stateProperty, BaseMessage msg)
    {
        Debug.Log("现在是NormalState");
        return stateLabel;
    }
}

