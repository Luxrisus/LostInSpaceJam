using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftWidget : MonoBehaviour
{
    public Image CraftSprite;
    public Image Background;
    public Transform QuantityWidgetContainer;
    public Text TimeTakenForCraft;

    public Color EnoughResourcesColor;
    public Color NotEnoughResourcesColor;

    [SerializeField]
    private QuantityWidget _quantityWidgetPrefab = null;

    private List<QuantityWidget> _quantities = new List<QuantityWidget>();

    public void Configure(Blueprint blueprint, Dictionary<Resources, int> availableResources)
    {
        CraftSprite.sprite = blueprint.CraftResult;

        foreach (var ingredient in blueprint.Ingredients)
        {
            var quantityWidget = Instantiate(_quantityWidgetPrefab, QuantityWidgetContainer);
            quantityWidget.Configure(ingredient.Display.Sprite, availableResources[ingredient.Display.Resource], ingredient.Quantity);

            _quantities.Add(quantityWidget);
        }

        if (!HasEnoughResources())
        {
            Background.color = NotEnoughResourcesColor;
            //Background.color = Color.yellow;
        }
        else
        {
            //Background.color = new Color(0, 125, 76, 255);
            Background.color = EnoughResourcesColor;
        }

        TimeTakenForCraft.text = $"Time:\n{blueprint.CraftTimeInSeconds}s";
    }

    public bool HasEnoughResources()
    {
        foreach(var quantity in _quantities)
        {
            if (!quantity.HasEnoughResources)
            {
                return false;
            }
        }

        return true;
    }

    public void Clear()
    {
        foreach(var quantity in _quantities)
        {
            Destroy(quantity.gameObject);
        }

        _quantities.Clear();
    }
}
