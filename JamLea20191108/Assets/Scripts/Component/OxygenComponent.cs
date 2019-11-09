using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OxygenComponent : MonoBehaviour
{
#region variables
    private float _oxygen;
    public float DepletionRatePerSecond;    //!< How much oxygen is depleted per second

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
    }

    // Update is called once per frame
    void Update()
    {
        _oxygen -= DepletionRatePerSecond * Time.deltaTime;
        if (_oxygen <= 0.0f)
            _oxygen = 0.0f;
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
