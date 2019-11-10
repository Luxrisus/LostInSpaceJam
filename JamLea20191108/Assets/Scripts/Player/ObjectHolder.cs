using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField]
    private ATransportableElement _currentElementInPossession = null;

    public void Take(ATransportableElement element)
    {
        element.Take(this);
        _currentElementInPossession = element;

        // Create a link if possible
        Linker linker = GetComponent<Linker>();
        if (linker != null)
        {
            ILinkable linkable = element.GetComponent<ILinkable>();
            if (linkable != null && !linkable.IsLinked())
            {
                linker.AddLink(linkable);
            }
        }
    }

    public ATransportableElement GetCurrentTransportableElement()
    {
        return _currentElementInPossession;
    }

    public void RemoveTransportableElement()
    {
        // Remove the link if possible
        ILinkable linkable = _currentElementInPossession.GetComponent<ILinkable>();
        if (linkable != null)
        {
            Linker linker = GetComponent<Linker>();
            if (linker != null && linkable.IsLinked())
            {
                linker.RemoveLink(linkable);
            }
        }

        _currentElementInPossession.Release();
        _currentElementInPossession = null;
    }

    public void TakeFrom(ObjectHolder holder)
    {
        ATransportableElement element = holder.GetCurrentTransportableElement();
        holder.RemoveTransportableElement();
        Take(element);
    }

    public bool HasObject()
    {
        return _currentElementInPossession != null;
    }
}
