using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATransportableElement : MonoBehaviour
{    
    ObjectHolder _holder = null;

    public void Take(ObjectHolder holder)
    {
        if (_holder != null)
        {
            _holder.RemoveTransportableElement();
        }
        _holder = holder;
        this.transform.SetParent(holder.transform);
    }

    public void Release()
    {
        _holder = null;
        this.transform.SetParent(null);
    }

    public bool IsCarried()
    {
        return _holder != null;
    }
}
