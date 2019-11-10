using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : ATransportableElement
{
    [SerializeField]
    private float _quantity = 1f;

    public float Quantity { get { return _quantity; } }
}
