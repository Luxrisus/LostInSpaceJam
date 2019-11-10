using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATransportableElement : MonoBehaviour, IInteractable
{
    static protected Transform _levelLayout;
    ObjectHolder _holder = null;
    Rigidbody2D _rigidBody = null;

    #region IInteractable
    public virtual void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
#endregion

    protected virtual void Awake()
    {
        _rigidBody = GetComponentInChildren<Rigidbody2D>();

        if (_levelLayout == null)
        {
            _levelLayout = FindObjectOfType<LevelLayoutHook>().transform;
        }
    }

    public virtual void Take(ObjectHolder holder)
    {
        if (_holder != null)
        {
            _holder.RemoveTransportableElement();
        }
        _holder = holder;
        this.transform.SetParent(holder.transform);
        _rigidBody.simulated = false;
    }

    public virtual void Release()
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
