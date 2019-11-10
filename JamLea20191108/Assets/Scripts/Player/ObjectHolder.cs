using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnObjectMovedEvent : UnityEvent<ATransportableElement>
{
}

public class ObjectHolder : MonoBehaviour
{
    [SerializeField]
    private ATransportableElement _currentElementInPossession = null;

    public OnObjectMovedEvent OnObjectTaken = new OnObjectMovedEvent();
    public OnObjectMovedEvent OnObjectReleased = new OnObjectMovedEvent();

    public void Take(ATransportableElement element)
    {
        _currentElementInPossession = element;
        element.Take(this);

        bool isLinked = false;
        // Create a link if possible
        Linker linker = GetComponent<Linker>();
        if (linker != null)
        {
            ILinkable linkable = element.GetComponent<ILinkable>();
            if (linkable != null && !linkable.IsLinked())
            {
                linker.AddLink(linkable);
                isLinked = true;
            }
        }

        // TODO
        if (!isLinked)
        {
        }
        OnObjectTaken.Invoke(element);
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
        OnObjectReleased.Invoke(_currentElementInPossession);
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
