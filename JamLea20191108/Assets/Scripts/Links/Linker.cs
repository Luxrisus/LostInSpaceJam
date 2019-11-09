﻿using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Link _linkPrefab;

    [SerializeField]
    int _maxLink = 3;
    
    [SerializeField]
    float _maxDistance = 10f;

    float _distanceTotal = 0f;
    
    public class LinkData
    {
        public Linker LinkStart;
        public ILinkable LinkEnd;
        public Link LinkObject;
        
        public LinkData(Linker linkStart, ILinkable linkEnd, Link linkObject)
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

    void FixedUpdate()
    {
        _distanceTotal = 0f;
        foreach (LinkData linkData in _links)
        {
            linkData.Update();
            _distanceTotal += linkData.LinkObject.GetDistance();
        }
    }

    bool CanAddLink()
    {
        return _links.Count < _maxLink;
    }
    
    public bool AddLink(ILinkable linkable)
    {
        bool canAddLink = CanAddLink();

        if (canAddLink)
        {
            Link link = Instantiate(_linkPrefab);
            _links.Add(new LinkData(this, linkable, link));
        }
        return canAddLink;
    }
    
    public bool RemoveLink(ILinkable linkable)
    {
        LinkData data = _links.Find(l => (l.LinkEnd == linkable));
        bool linkRemoved = data != null;
        
        if (linkRemoved)
        {
            _links.Remove(data);
            Destroy(data.LinkObject.gameObject);
        }
        return linkRemoved;
    }

    public Vector3 GetCorrectedTranslation(ILinkable linkable, Vector3 translation)
    {
        LinkData linkData = _links.Find(l => (l.LinkEnd == linkable));

        if (linkData != null)
        {
            float currentDistance = linkData.LinkObject.GetDistance();
            float nextDistance = (linkData.LinkEnd.GetPosition() + translation).magnitude;

            float diffDistance = nextDistance - currentDistance;
            if ((_distanceTotal + diffDistance) > _maxDistance)
            {
                Vector3 newPos = (linkable.GetPosition() + translation - GetPosition()).normalized * currentDistance;
                translation = newPos - linkable.GetPosition();
            }
        }
        return translation;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

#region IInteractable
    public void DoInteraction(Player player)
    {
        ILinkable linkable = player.GetComponent<ILinkable>();
        if (linkable != null)
        {
            AddLink(linkable);
        }
    }
#endregion
}
