using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelShuttle : MonoBehaviour, IInteractable
{
    ObjectHolder _holder = null;

    void Start()
    {
        _holder = GetComponent<ObjectHolder>();
        _holder.OnObjectTaken.AddListener(PutPlant);
    }

    public void DoInteraction(Player player)
    {
        // We need ti implement interactable to allo the player to interact with us
        // we should rework this
    }

    public void PutPlant(ATransportableElement element)
    {
        Plant plant = (Plant)element;
        if (plant != null)
        {
            ManagersManager.Instance.Get<LevelManager>().EndOfLevel(true);
            // TODO @Salanyel: Trigger the end of the current game
            Debug.Log("Plant passed!");
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
