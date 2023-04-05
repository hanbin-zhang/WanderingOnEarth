using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public abstract class BaseEvent { 
    
}

[Serializable]
public abstract class BaseMessage
{
    public T Of<T>() where T : BaseMessage => (T)this;
}
