using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private Image _oxygenBar;

    public void SetColor(Color color)
    {
        _oxygenBar.color = color;
    }

    public void SetFillRatio(float value)
    {
        _oxygenBar.fillAmount = value;
    }
}
