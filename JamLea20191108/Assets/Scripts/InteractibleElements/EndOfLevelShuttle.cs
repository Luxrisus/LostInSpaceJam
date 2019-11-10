using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelShuttle : MonoBehaviour, IInteractable
{
    public void DoInteraction(Player player)
    {
        Debug.Log("Test");
        
        ATransportableElement currentElement = null;
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            currentElement = holder.GetCurrentTransportableElement();
        }

        Plant plant = (Plant)currentElement;
        if (plant != null)
        {
            PutPlant(plant);
        }
    }

    public void PutPlant(Plant plant)
    {
        // TODO @Salanyel: Trigger the end of the current game
        Debug.Log("Plant passed!");
    }

    public bool CanInteract()
    {
        return true;
    }
}
