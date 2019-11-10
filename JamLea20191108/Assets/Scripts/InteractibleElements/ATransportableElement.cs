﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATransportableElement : MonoBehaviour
{
    static Transform _levelLayout;
    ObjectHolder _holder = null;
    Rigidbody2D _rigidBody = null;

    protected virtual void Awake()
    {
        _rigidBody = GetComponentInChildren<Rigidbody2D>();

        if (_levelLayout == null)
        {
            _levelLayout = FindObjectOfType<LevelLayoutHook>().transform;
        }
    }

    public void Take(ObjectHolder holder)
    {
        if (_holder != null)
        {
            _holder.RemoveTransportableElement();
        }
        _holder = holder;
        this.transform.SetParent(holder.transform);
        _rigidBody.simulated = false;
    }

    public void Release()
    {
        _holder = null;
        this.transform.SetParent(_levelLayout);
        _rigidBody.simulated = true;
    }

    public bool IsCarried()
    {
        return _holder != null;
    }
}
