using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevelShuttle : MonoBehaviour, IInteractable
{
    public bool IsDetectingPlayerOnCollision = false;

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
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDetectingPlayerOnCollision)
        {
            var player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                ManagersManager.Instance.Get<LevelManager>().EndOfLevel(true);
            }
        }
    }
}
