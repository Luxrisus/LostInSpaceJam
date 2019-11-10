using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTimeHolder : ATransportableElement
{
    public GameObject _objectToInstantiate;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Release()
    {
        GameObject objectInstantiated = Instantiate(_objectToInstantiate, transform.position, transform.rotation);
        objectInstantiated.transform.SetParent(_levelLayout);
        Destroy(gameObject);
    }
}
