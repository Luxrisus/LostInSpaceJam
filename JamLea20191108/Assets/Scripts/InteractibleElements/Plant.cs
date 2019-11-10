using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : ATransportableElement, IInteractable, ILinkable
{
    [SerializeField]
    private PlantOxygenHud _oxygenHud = null;

    private Linker _linker = null;
    private OxygenComponent _oxygenComponent = null;

    void Start()
    {
        _oxygenComponent = GetComponent<OxygenComponent>();
    }

    private void Update()
    {
        if (_oxygenComponent.OxygenLevel <= 0)
        {
            ManagersManager.Instance.Get<LevelManager>().EndOfLevel(false);
            Destroy(gameObject);
        }

        _oxygenHud.OxygenIndicator.fillAmount = (float)_oxygenComponent.OxygenLevel / (float)_oxygenComponent.OxygenMax;
    }

    #region IInteractable
    public void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
#endregion

    public void OnUnlink(Linker owner)
    {
        _linker = null;
        _oxygenComponent.Plugged = false;
    }

    public void OnLink(Linker linker)
    {
        _linker = linker;
        _oxygenComponent.Plugged = true;
    }

    public void Unlink()
    {
        if (IsLinked())
        {
            _linker.RemoveLink(this);
        }
    }

    public bool IsLinked()
    {
        return _linker != null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
