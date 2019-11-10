using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WaterComponent : MonoBehaviour
{
    private float _water;
    public float DepletionRatePerSecond = 1f;    //!< How much water is depleted per second
    //public float RefillRatePerSecond = 10.0f;    //!< How much oxygen is depleted per second
    public float WaterMovementDepletionFactor = 1.25f;
    //public bool Plugged;    //!< When plugged, oxgen refills. Unplugged, oxygen expires
    private Vector3 _lastPosition;
    public int WaterMax;           //!< Maximum amount of oxygen

    void Start()
    {
        WaterLevel = WaterMax;
        _lastPosition = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ManagersManager.Instance.Get<LevelManager>().getLevelState() != LevelState.EndOfGame)
        {
            float depletion;

            if (transform.localPosition != _lastPosition)
            {
                depletion = DepletionRatePerSecond * WaterMovementDepletionFactor;
            }
            else
            {
                depletion = DepletionRatePerSecond;
            }

            _lastPosition = transform.localPosition;

            depletion *= Time.deltaTime;
            _water -= depletion;
            if (_water <= 0.0f)
                _water = 0.0f;
        }
    }

    public int WaterLevel
    {
        get { return Mathf.RoundToInt(_water); }
        set
        {
            Assert.IsTrue(value >= 0.0f);
            _water = value;
        }
    }

    public int WaterPercentage
    {
        get
        {
            return Mathf.RoundToInt(_water / WaterMax);
        }
    }

    public bool IsFull()
    {
        return WaterPercentage == 100;
    }

    public bool IsEmpty()
    {
        return _water <= 0.0f;
    }
}
