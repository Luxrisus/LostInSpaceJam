using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OxygenComponent : MonoBehaviour
{
#region variables
    private float _oxygen;
    public float DepletionRatePerSecond = 1.0f;    //!< How much oxygen is depleted per second
    public float RefillRatePerSecond = 10.0f;    //!< How much oxygen is depleted per second

    public bool Plugged;    //!< When plugged, oxgen refills. Unplugged, oxygen expires

    //!< Current oxygen level
    public int OxygenLevel
    {
        get { return Mathf.RoundToInt(_oxygen); }
        set
        {
            Assert.IsTrue(value >= 0.0f);
            _oxygen = value;
        }
    }
    public int OxygenMax;           //!< Maximum amount of oxygen

    //!< Percentage of total oxygen capacity
    public int OxygenPercentage
    {
        get
        {
            return Mathf.RoundToInt(_oxygen / OxygenMax);
        }
    }
#endregion

#region publicMethods
    void Start()
    {
        OxygenLevel = OxygenMax;
        Plugged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Plugged)
        {
            _oxygen += RefillRatePerSecond * Time.deltaTime;
            if (_oxygen >= OxygenMax)
                _oxygen = OxygenMax;
        }
        else
        {
            _oxygen -= DepletionRatePerSecond * Time.deltaTime;
            if (_oxygen <= 0.0f)
                _oxygen = 0.0f;
        }
    }

    public bool IsFull()
    {
        return OxygenPercentage == 100;
    }

    public bool IsEmpty()
    {
        return _oxygen <= 0.0f;
    }

#endregion
}
