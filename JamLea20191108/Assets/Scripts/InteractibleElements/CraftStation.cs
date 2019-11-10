using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftStation : ATransportableElement, IInteractable
{
    ObjectHolder _holder;

    void Start()
    {
        _holder = GetComponent<ObjectHolder>();
        _holder.OnObjectTaken.AddListener(OnObjectTaken);
    }

    public void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
    }

    void OnObjectTaken(ATransportableElement element)
    {
        Plant plant = (Plant)element;

        if (plant)
        {
            // We can't put the plant in the craft station
            _holder.RemoveTransportableElement();
        }
        else
        {
            // Delete the gameobject and store it for the craft
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
