using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTimeHolder : ATransportableElement, IInteractable
{
    public GameObject _objectToInstantiate;

    void Start()
    {
        
    }

    public void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
    }

    public override void Release()
    {
        Instantiate(_objectToInstantiate, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public bool CanInteract()
    {
        return true;
    }
}
