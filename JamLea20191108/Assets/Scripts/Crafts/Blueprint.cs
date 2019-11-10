using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDisplay", menuName = "Craftable Elements/Ingredient", order = 1)]
public class IngredientDisplay : ScriptableObject
{
    public Sprite Sprite;
    public Resources Resource;
}

[System.Serializable]
public struct Ingredient
{
    public IngredientDisplay Display;
    public int Quantity;
}

public enum Resources
{
    Ice = 0,
    Wood,
}

[CreateAssetMenu(fileName = "Blueprint", menuName = "Craftable Elements/Blueprint", order = 1)]
public class Blueprint : ScriptableObject
{
    public ATransportableElement Result;
    public Sprite CraftResult;
    public List<Ingredient> Ingredients;
    public float CraftTimeInSeconds;
}
