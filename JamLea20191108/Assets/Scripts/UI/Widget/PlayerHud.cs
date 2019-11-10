using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private Image _playersElement = null;

    [SerializeField]
    private Image _oxygenBar = null;

    public void SetColor(Color color)
    {
        _playersElement.color = color;
        _oxygenBar.color = color;
    }

    public void SetFillRatio(float value)
    {
        _oxygenBar.fillAmount = value;
    }
}
