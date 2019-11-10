using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : ATransportableElement, ILinkable
{
    [SerializeField]
    private PlantOxygenHud _oxygenHud = null;
    [SerializeField]
    private PlantWaterHud _waterHud = null;

    private Linker _linker = null;
    private OxygenComponent _oxygenComponent = null;

    private WaterComponent _waterComponent = null;

    void Start()
    {
        _oxygenComponent = GetComponent<OxygenComponent>();
        _waterComponent = GetComponent<WaterComponent>();
    }

    private void Update()
    {
        if (_oxygenComponent.OxygenLevel <= 0)
        {
            ManagersManager.Instance.Get<LevelManager>().EndOfLevel(false);
            Destroy(gameObject);
        }

        _oxygenHud.OxygenIndicator.fillAmount = (float)_oxygenComponent.OxygenLevel / (float)_oxygenComponent.OxygenMax;

        if (_waterComponent.WaterLevel <= 0)
        {
            ManagersManager.Instance.Get<LevelManager>().EndOfLevel(false);
            Destroy(gameObject);
        }

        _waterHud.WaterIndicator.fillAmount = (float)_waterComponent.WaterLevel / (float)_waterComponent.WaterMax;
    }

    public bool AddWater(Water water)
    {
        if (water != null)
        {
            _waterComponent.Add(water.Quantity);
            Destroy(water.gameObject);
            return true;
        }
        return false;
    }

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
