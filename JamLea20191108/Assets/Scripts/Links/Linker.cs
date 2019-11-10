using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Link _linkPrefab = null;

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

    public bool IsLinked(ILinkable linkable)
    {
        return _links.Find(l => (l.LinkEnd == linkable)) != null;
    }

    public bool AddLink(ILinkable linkable)
    {
        bool canAddLink = CanAddLink();

        if (canAddLink)
        {
            Link link = Instantiate(_linkPrefab);
            link.transform.SetParent(transform);
            _links.Add(new LinkData(this, linkable, link));
            linkable.OnLink(this);
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
            linkable.OnUnlink(this);
        }
        return linkRemoved;
    }

    public Vector3 GetCorrectedTranslation(ILinkable linkable, Vector3 translation)
    {
        LinkData linkData = _links.Find(l => (l.LinkEnd == linkable));

        if (linkData != null)
        {
            float currentDistance = linkData.LinkObject.GetDistance();
            float nextDistance = Vector3.Distance(linkData.LinkObject.GetStart(), linkData.LinkObject.GetEnd() + translation);

            float diffDistance = nextDistance - currentDistance;
            if ((_distanceTotal + diffDistance) > _maxDistance)
            {
                Vector3 direction = linkData.LinkObject.GetEnd() - linkData.LinkObject.GetStart();
                Vector3 newPos = (direction + translation).normalized * currentDistance;
                translation = newPos - direction;
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
        if (IsLinked(player))
        {
            RemoveLink(player);
        }
        else
        {
            AddLink(player);
        }
    }

    public bool CanInteract()
    {
        return _links.Count < _maxLink;
    }
    #endregion

    private void OnDestroy()
    {
        var linksToDelete = new List<ILinkable>();

        foreach(var link in _links)
        {
            linksToDelete.Add(link.LinkEnd);
        }

        foreach(var linkToDelete in linksToDelete)
        {
            RemoveLink(linkToDelete);
        }

        linksToDelete.Clear();
    }
}
