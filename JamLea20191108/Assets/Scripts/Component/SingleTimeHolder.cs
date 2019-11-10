using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTimeHolder : ATransportableElement
{
    public GameObject _objectToInstantiate;

    void Start()
    {
        
    }

    public override void Release()
    {
        Instantiate(_objectToInstantiate, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
