using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILinkable
{
    //void OnLinkadded(Linker owner);
    //void OnLinkDestroyed(Linker owner);
    Vector3 GetPosition();

    //bool CanMove(); apply restrictions based on distance and max count
}

public class Linker : MonoBehaviour, ILinkable
{
    [SerializeField]
    private Link _linkPrefab;

    [SerializeField]
    int _maxLink = 3;
    
    [SerializeField]
    float _maxDistance = 10f;
    
    public struct LinkData
    {
        public ILinkable LinkStart;
        public ILinkable LinkEnd;
        public Link LinkObject;
        
        public LinkData(ILinkable linkStart, ILinkable linkEnd, Link linkObject)
        {
            LinkStart = linkStart;
            LinkEnd = linkEnd;
            LinkObject = linkObject;

        }

        public void Update()
        {
            LinkObject.UpdatePosition(LinkStart.GetPosition(), LinkEnd.GetPosition());
        }
    }

    List<LinkData> _links = new List<LinkData>();

    void Start()
    {
        
    }

    void Update()
    {
        foreach (LinkData linkData in _links)
        {
            linkData.Update();
        }
    }
    
    public void AddLink(ILinkable linkable)
    {
        Link link = Instantiate(_linkPrefab);

        _links.Add(new LinkData(this, linkable, link));
    }
    
    public void RemoveLink(ILinkable linkable)
    {
        //todo
        _links.RemoveAll(l => (l.LinkStart == linkable || l.LinkEnd == linkable));
    }
    /*
    public void OnLinkadded(Linker owner)
    {

    }

    public void OnLinkDestroyed(Linker owner)
    {

    }*/

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
