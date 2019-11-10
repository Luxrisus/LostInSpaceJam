﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftStation : ATransportableElement
{
    [SerializeField]
    private GameObject _craftWidgetCanvas = null;
    [SerializeField]
    private CraftWidget _craftWidget = null;

    static private Dictionary<Resources, int> _resources;

    private List<Blueprint> _blueprints = new List<Blueprint>();
    private int _numberOfCollisions = 0;
    private int _currentBlueprintIndex = 0;
    private Coroutine _craftCor = null;
    private Player _craftingPlayer = null;

    protected override void Awake()
    {
        base.Awake();

        if (_resources == null)
        {
            _resources = new Dictionary<Resources, int>();
            _resources.Add(Resources.Ice, 2);
            _resources.Add(Resources.Wood, 0);
        }

        _craftWidgetCanvas.SetActive(false);

        GetComponent<ObjectHolder>().OnObjectTaken.AddListener(OnObjectTaken);
    }

    void Start()
    {
        _blueprints = ManagersManager.Instance.Get<CraftManager>().GetBlueprints();
    }

    public void StartCraft(Player player)
    {
        if (_craftCor == null && !player.GetComponent<ObjectHolder>().HasObject())
        {
            Blueprint blueprint = GetCurrentBlueprint();
            if (CanCraft(blueprint))
            {
                Debug.Log("Start craft");
                _craftingPlayer = player;
                _craftCor = StartCoroutine(CraftCor(_craftingPlayer, blueprint));
            }
        }
    }

    public void StopCraft(Player player)
    {
        if (player == _craftingPlayer && _craftCor != null)
        {
            Debug.Log("Stop craft");
            StopCoroutine(_craftCor);
        }
    }

    IEnumerator CraftCor(Player player, Blueprint blueprint)
    {
        float timer = 0f;
        while (timer < blueprint.CraftTimeInSeconds)
        {
            timer += Time.deltaTime;
            // TODO update the UI with ratio = timer / blueprint.CraftTimeInSeconds
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Craft done");
        ConsumeIngredient(blueprint);
        ATransportableElement result = Instantiate(blueprint.Result);
        _craftingPlayer.GetComponent<ObjectHolder>().Take(result);
        DisplayBlueprint();
    }

    void OnObjectTaken(ATransportableElement element)
    {
        if (element.GetType() == typeof(Plant))
        {
            // We can't put the plant in the craft station
            GetComponent<ObjectHolder>().RemoveTransportableElement();
        }
        else if (element.GetType() == typeof(CraftableComponent))
        {
            var craftableComponent = (CraftableComponent)(element);

            _resources[craftableComponent.GetResource()]++;
            Destroy(element.gameObject);
            DisplayBlueprint();
        }
    }

    public bool CanCraft(Blueprint blueprint)
    {
        bool canCraft = true;
        foreach (Ingredient ingredient in blueprint.Ingredients)
        {
            int quantity = 0;
            if (!_resources.TryGetValue(ingredient.Display.Resource, out quantity) || quantity < ingredient.Quantity)
            {
                canCraft = false;
                break;
            }
        }

        return canCraft;
    }

    public void ConsumeIngredient(Blueprint blueprint)
    {
        foreach (Ingredient ingredient in blueprint.Ingredients)
        {
            _resources[ingredient.Display.Resource] -= ingredient.Quantity;
        }
    }

    public Blueprint GetCurrentBlueprint()
    {
        return _blueprints[_currentBlueprintIndex];
    }

    public void NextBlueprint()
    {
        _currentBlueprintIndex++;
        if (_currentBlueprintIndex >= _blueprints.Count)
        {
            _currentBlueprintIndex = 0;
        }
        DisplayBlueprint();
    }

    public void PrevBlueprint()
    {
        _currentBlueprintIndex--;
        if (_currentBlueprintIndex < 0)
        {
            _currentBlueprintIndex = _blueprints.Count - 1;
        }
        DisplayBlueprint();
    }

    #region collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _numberOfCollisions++;

            if (_numberOfCollisions == 1)
            {
                _craftWidgetCanvas.SetActive(true);
                _craftWidget.Configure(GetCurrentBlueprint(), _resources);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _numberOfCollisions--;

            if (_numberOfCollisions == 0)
            {
                _currentBlueprintIndex = 0;
                _craftWidget.Clear();
                _craftWidgetCanvas.SetActive(false);
            }
        }
    }
    #endregion

    private void DisplayBlueprint()
    {
        _craftWidget.Clear();
        _craftWidget.Configure(GetCurrentBlueprint(), _resources);
    }
}
