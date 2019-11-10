using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDisplay", menuName = "Craftable Elements/Ingredient", order = 1)]
public class IngredientDisplay : ScriptableObject
{
    public Sprite Sprite;
    public Resources Resource;
}
