using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : MonoBehaviour
{
    protected Linker _linker = null;
    private ObjectHolder _objectHolder;

    void Start()
    {
        _objectHolder = GetComponent<ObjectHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
