using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour, ILinkable, IInteractable
{
    [SerializeField]
    private Link _linkPrefab;

    [SerializeField]
    int _maxLink = 3;
    
    [SerializeField]
    float _maxDistance = 10f;
    
    public class LinkData
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
        LinkData data = _links.Find(l => (l.LinkEnd == linkable || l.LinkStart == linkable));
        
        if (data != null)
        {
            _links.Remove(data);
            Destroy(data.LinkObject.gameObject);
        }
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

    public bool IsLinked()
    {
        return _links.Count > 0;
    }

    public void DoInteraction(Player player)
    {
        ILinkable linkable = player.GetComponent<ILinkable>();
        if (linkable != null)
        {
            AddLink(linkable);
        }
    }
}
