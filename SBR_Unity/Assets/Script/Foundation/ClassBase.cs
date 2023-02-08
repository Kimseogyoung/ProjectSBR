using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ClassBase
{

    public ClassBase()
    {
        Start();
    }

    abstract protected void Start();
    virtual protected void Stop()
    {

    }
    virtual protected void Restart()
    {

    }
    abstract protected void Finished();
}
