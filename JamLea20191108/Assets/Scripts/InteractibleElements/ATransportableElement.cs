using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATransportableElement : MonoBehaviour
{    
    public void Take(Transform jointToAssign)
    {
        this.transform.SetParent(jointToAssign);
    }

    public void Put()
    {
        this.transform.SetParent(null);
    }   
}
