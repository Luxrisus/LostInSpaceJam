using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ILinkable
{
#region variables
    [SerializeField]
    private float _speed = 5f;

    private Vector3 _direction = Vector3.zero;
    private Linker _linker = null;
    private ObjectHolder _objectHolder;

    [SerializeField]
    private OxygenComponent _oxygenComponent = null;

    private List<GameObject> _interactablesElement = new List<GameObject>();
    private SpriteRenderer _spriteRenderer;
    private PlayerHud _playerHud;
    public bool _isMainActionPressed = false;

#endregion

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ManagersManager.Instance.Get<PlayerManager>().PlayerJoined(this);
    }

    void Start()
    {
        Assert.IsNotNull(_oxygenComponent);
        _objectHolder = GetComponent<ObjectHolder>();
    }

    void Update()
    {
        float ratio = (float)_oxygenComponent.OxygenLevel / (float)_oxygenComponent.OxygenMax;
        _playerHud?.SetFillRatio(ratio);
    }

    void FixedUpdate()
    {
        Vector3 translation = _speed * Time.deltaTime * _direction;

        if (_linker != null)
        {
            translation = _linker.GetCorrectedTranslation(this, translation);
        }
        transform.Translate(translation);

        if (_oxygenComponent.OxygenLevel == 0)
        {
            Die();
        }
    }

#region Input Management

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        _direction = new Vector3(move.x, move.y, 0f);
    }

    // Only interact with Linker
    public void OnLinkAction(InputValue value)
    {
        if (_linker != null)
        {
            Unlink();
        }
        else
        {
            foreach (GameObject element in _interactablesElement)
            {
                Linker linker = element.GetComponent<Linker>();
                if (linker != null && linker.CanInteract())
                {
                    linker.DoInteraction(this);
                }
            }
        }
    }

    // Interact with every IInteractable except Linker
    public void OnHoldAction(InputValue value)
    {
        // If the player carry something try to give the object
        if (_objectHolder != null && _objectHolder.HasObject())
        {
            ObjectHolder newHolder = null;
            foreach (GameObject element in _interactablesElement)
            {
                newHolder = element.GetComponent<ObjectHolder>();
                if (newHolder != null)
                {
                    break;
                }
            }

            if (newHolder != null)
            {
                newHolder.TakeFrom(_objectHolder);
            }
            else
            {
                _objectHolder.RemoveTransportableElement();
            }
        }
        else
        {
            // If the player is not carrying an object
            if (_objectHolder != null && !_objectHolder.HasObject())
            {
                foreach (GameObject element in _interactablesElement)
                {
                    IInteractable interactable = element.GetComponent<IInteractable>();
                    Linker linker = element.GetComponent<Linker>();

                    // if it's a linker try to take the object from it
                    if (linker != null)
                    {
                        ObjectHolder holder = linker.GetComponent<ObjectHolder>();
                        if (holder != null)
                        {
                            _objectHolder.TakeFrom(holder);
                        }
                    }
                    else if (interactable != null && interactable.CanInteract())
                    {
                        interactable.DoInteraction(this);
                    }
                }
            }
        }
    }

    public void OnMainAction(InputValue value)
    {
        _isMainActionPressed = value.isPressed;

    }
    
    public void OnChangeIngredientLeft(InputValue value)
    {
        Debug.Log("TODO: OnChangeIngredientLeft");
    }
    
    public void OnChangeIngredientRight(InputValue value)
    {
        Debug.Log("TODO: OnChangeIngredientRight");
    }

#endregion

#region Linking Logic

    public void OnUnlink(Linker owner)
    {
        Assert.IsTrue(IsLinked());
        _linker = null;
        _oxygenComponent.Plugged = false;
        Debug.Log("Houston, this is Jacky ! Holding my respiration ! Ovaire !");
    }

    public void OnLink(Linker linker)
    {
        _linker = linker;
        _oxygenComponent.Plugged = true;
    }

    public void Unlink()
    {
        if (IsLinked())
        {
            _linker.RemoveLink(this);
        }
    }

    public bool IsLinked()
    {
        return _linker != null;
    }
 #endregion

    public void Die()
    {
        Unlink();

        if (_objectHolder.HasObject())
        {
            _objectHolder?.RemoveTransportableElement();
        }
        ManagersManager.Instance.Get<PlayerManager>().PlayerDied(this);
        ManagersManager.Instance.Get<UIManager>().PlayersPanel.PlayerDied(_playerHud);

        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }

    public void AddPlayerHud(PlayerHud hud)
    {
        _playerHud = hud;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

#region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();
        if (newInteractable != null)
        {
            _interactablesElement.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();
        if (newInteractable != null)
        {
            _interactablesElement.Remove(collision.gameObject);
        }
    }
#endregion

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

}

