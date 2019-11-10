using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantityWidget : MonoBehaviour
{
    [SerializeField]
    private Image _elementSprite = null;
    [SerializeField]
    private Text _currentQuantity = null;
    [SerializeField]
    private Text _quantityRequired = null;

    public bool HasEnoughResources = true;

    public void Configure(Sprite elementSprite, int currentQuantity, int requiredQuantity)
    {
        _elementSprite.sprite = elementSprite;
        _currentQuantity.text = currentQuantity.ToString();

        if (currentQuantity < requiredQuantity)
        {
            _currentQuantity.color = Color.red;
            HasEnoughResources = false;
        }
        else
        {
            _currentQuantity.color = Color.black;
            HasEnoughResources = true;
        }

        _quantityRequired.text = requiredQuantity.ToString();
    }



}
