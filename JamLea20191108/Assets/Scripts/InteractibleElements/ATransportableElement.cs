using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATransportableElement : MonoBehaviour
{    
    Transform _owner = null;

    public void Take(Transform jointToAssign)
    {
        _owner = jointToAssign;
        this.transform.SetParent(jointToAssign);
    }

    public void Release()
    {
        _owner = null;
        this.transform.SetParent(null);
    }

    public bool IsCarried()
    {
        return _owner != null;
    }
}
