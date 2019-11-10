using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftStation : ATransportableElement, IInteractable
{
    [SerializeField]
    private GameObject _craftWidgetCanvas = null;
    [SerializeField]
    private CraftWidget _craftWidget = null;

    static private Dictionary<Resources, int> _resources;

    private ObjectHolder _holder;
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
            _resources.Add(Resources.Ice, 0);
            _resources.Add(Resources.Wood, 2);
        }

        _craftWidgetCanvas.SetActive(false);

        _holder = GetComponent<ObjectHolder>();
        _holder.OnObjectTaken.AddListener(OnObjectTaken);
    }

    void Start()
    {
        _blueprints = ManagersManager.Instance.Get<CraftManager>().GetBlueprints();
    }

    public void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
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
        // Remove ingredients
        // Give the object to the player
    }

    void OnObjectTaken(ATransportableElement element)
    {
        Plant plant = (Plant)element;

        if (plant)
        {
            // We can't put the plant in the craft station
            _holder.RemoveTransportableElement();
        }
        else
        {
            // Delete the gameobject and store it for the craft
        }
    }

    public bool CanInteract()
    {
        return true;
    }

    public bool CanCraft(Blueprint blueprint)
    {
        return true;
    }

    public Blueprint GetCurrentBlueprint()
    {
        return _blueprints[_currentBlueprintIndex];
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

    public void DisplayNextBlueprint()
    {
        if (_blueprints.Count < 2)
        {
            return;
        }

        _currentBlueprintIndex = _currentBlueprintIndex == _blueprints.Count - 1 ? 0 : _currentBlueprintIndex + 1;
        DisplayBlueprint();
    }

    public void DisplayPreviousBlueprint()
    {
        if (_blueprints.Count < 2)
        {
            return;
        }

        _currentBlueprintIndex = _currentBlueprintIndex == 0 ? _blueprints.Count - 1 : _currentBlueprintIndex - 1;
        DisplayBlueprint();
    }
}
