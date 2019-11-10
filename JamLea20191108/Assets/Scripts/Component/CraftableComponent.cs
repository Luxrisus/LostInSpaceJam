using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableComponent : ATransportableElement
    {

    [SerializeField]
    private IngredientDisplay _ingredientDisplay;
    [SerializeField]
    private SpriteRenderer _sprite;

    private void Awake()
    {
        this._sprite.sprite = _ingredientDisplay.Sprite;
    }
}