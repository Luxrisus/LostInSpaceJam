using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : ATransportableElement, IInteractable
{
#region IInteractable
    public void DoInteraction(Player player)
    {
        Debug.Log("interaction plant");
        if (player.GetCurrentTransportableElement() == this)
        {
            player.RemoveTransportableElement();
        }
        else
        {
            player.Take(this);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
#endregion
}
