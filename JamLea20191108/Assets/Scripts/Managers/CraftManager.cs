using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : AManager
{
    [SerializeField]
    private List<Blueprint> _blueprints = new List<Blueprint>();

    [SerializeField]
    private List<ACraftable> _craftables = new List<ACraftable>();

    public override void Initialize()
    {
        var levelLayout = FindObjectOfType<LevelLayoutHook>();

        _blueprints = levelLayout.Blueprints;

        foreach(var ingredient in levelLayout.BaseIngredients)
        {
            CraftStation.AddRessources(ingredient);
        }
    }

    public List<Blueprint> GetBlueprints()
    {
        return _blueprints;
    }

    public List<ACraftable> GetCraftables()
    {
        return _craftables;
    }

    public T Get<T>() where T : ACraftable
    {
        foreach(var craftable in _craftables)
        {
            if (craftable.GetType() == typeof(T))
            {
                return (T)(craftable);
            }
        }

        return null;
    }
}
