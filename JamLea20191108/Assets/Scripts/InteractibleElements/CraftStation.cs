using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftStation : ATransportableElement
{
    [SerializeField]
    static private int initialResources = 0;
    [SerializeField]
    private GameObject _craftWidgetCanvas = null;
    [SerializeField]
    private CraftWidget _craftWidget = null;
    [SerializeField]
    private CraftBarWidget _craftBarWidget = null;
    static private Dictionary<Resources, int> _resources;

    private List<Blueprint> _blueprints = new List<Blueprint>();
    private int _numberOfCollisions = 0;
    private int _currentBlueprintIndex = 0;
    private Coroutine _craftCor = null;
    private Player _craftingPlayer = null;

    protected override void Awake()
    {
        base.Awake();

        InitializeResources();

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
                _craftBarWidget.Configure(0, blueprint.CraftTimeInSeconds);
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
            _craftCor = null;
            _craftBarWidget.Clear();
        }
    }

    IEnumerator CraftCor(Player player, Blueprint blueprint)
    {
        float timer = 0f;
        while (timer < blueprint.CraftTimeInSeconds)
        {
            timer += Time.deltaTime;
            _craftBarWidget.SetCurrentValue(timer);

            yield return null;
        }

        Debug.Log("Craft done");
        ConsumeIngredient(blueprint);
        ATransportableElement result = Instantiate(blueprint.Result, transform.position, transform.rotation);
        _craftingPlayer.GetComponent<ObjectHolder>().Take(result);
        _craftCor = null;
        _craftBarWidget.Clear();
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

    static private void InitializeResources()
    {
        if (_resources == null)
        {
            _resources = new Dictionary<Resources, int>();
            _resources.Add(Resources.Ice, initialResources);
            _resources.Add(Resources.Wood, initialResources);
        }
    }

    static public void AddRessources(Ingredient resource)
    {
        InitializeResources();

        _resources[resource.Display.Resource] += resource.Quantity;
    }

    static public void ResetCraftables()
    {
        _resources?.Clear();
        _resources = null;
    }
}
