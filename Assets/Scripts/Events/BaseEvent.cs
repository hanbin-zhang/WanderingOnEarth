using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class BaseEvent { }

public abstract class BaseMessage
{
    public EventLabel eventLabel;
    public T Of<T>() where T : BaseMessage => (T)this;
}
