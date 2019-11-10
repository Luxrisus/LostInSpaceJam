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
    }

    public ATransportableElement GetCurrentTransportableElement()
    {
        return _currentElementInPossession;
    }

    public void RemoveTransportableElement()
    {
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
