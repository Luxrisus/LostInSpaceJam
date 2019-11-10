using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableComponent : ATransportableElement, IInteractable
    {

    [SerializeField]
    private IngredientDisplay _ingredientDisplay = null;
    [SerializeField]
    private SpriteRenderer _sprite = null;

    public bool CanInteract()
    {
        return true;
    }

    public void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
    }

    public Resources GetResource()
    {
        return _ingredientDisplay.Resource;
    }

    [ExecuteInEditMode]
    protected override void Awake()
    {
        base.Awake();

        this._sprite.sprite = _ingredientDisplay.Sprite;
    }
}