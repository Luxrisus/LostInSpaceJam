using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftBarWidget : MonoBehaviour
{
    [SerializeField]
    private GameObject _contentDisplay = null;
    [SerializeField]
    private Image _fillValue = null;

    private float _currentValue;
    private float _targetValue;
    private bool _isProgressing = false;

    private void Awake()
    {
        Clear();
    }

    public void Configure(float currentValue, float targetValue)
    {
        _targetValue = targetValue;
        SetCurrentValue(targetValue);
        _fillValue.fillAmount = currentValue / targetValue;
        _contentDisplay.SetActive(true);
        _isProgressing = true;
    }

    public void SetCurrentValue(float currentValue)
    {
        _currentValue = currentValue;
    }

    public void Clear()
    {
        _contentDisplay.SetActive(false);
        _currentValue = 0;
        _targetValue = 0;
        _isProgressing = false;
    }

    private void Update()
    {
        if (_isProgressing)
        {
            _fillValue.fillAmount = _currentValue / _targetValue;
        }
    }
}
