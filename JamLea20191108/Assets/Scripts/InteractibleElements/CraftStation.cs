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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _numberOfCollisions++;

            if (_numberOfCollisions == 1)
            {
                _craftWidgetCanvas.SetActive(true);
                _craftWidget.Configure(_blueprints[_currentBlueprintIndex], _resources);
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
}
