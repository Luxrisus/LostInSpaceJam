using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ILinkable
{
    #region variables

    private const float _kOxygenRatioForAlarm = 0.4f;

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private Transform _wholeBodyController = null;

    [SerializeField]
    private SpriteRenderer _bodyRenderer = null;
    [SerializeField]
    private SpriteRenderer _mainRenderer = null;
    [SerializeField]
    private SpriteRenderer _secondaryRenderer = null;

    private Vector3 _direction = Vector3.zero;
    private Linker _linker = null;
    private ObjectHolder _objectHolder;

    [SerializeField]
    private OxygenComponent _oxygenComponent = null;

    private List<GameObject> _interactablesElement = new List<GameObject>();
    private PlayerHud _playerHud;
    private CraftStation _craftStation = null;
    private float _lastPositionX = 0f;
    private AudioSource _audioSource;
    private bool _isAudioPlaying = false;

#endregion

    void Awake()
    {
        ManagersManager.Instance.Get<PlayerManager>().PlayerJoined(this);
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.5f;
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
        _audioSource.clip = ManagersManager.Instance.Get<SoundManager>().GetAudioClip(SoundName.OxygenAlarmWithSilence);
    }

    void Start()
    {
        Assert.IsNotNull(_oxygenComponent);
        _objectHolder = GetComponent<ObjectHolder>();
    }

    void Update()
    {
        float ratio = (float)_oxygenComponent.OxygenLevel / (float)_oxygenComponent.OxygenMax;

        if (!_isAudioPlaying && ratio <= _kOxygenRatioForAlarm)
        {
            _audioSource.Play();
            _isAudioPlaying = true;
        }
        else if (_isAudioPlaying && ratio > _kOxygenRatioForAlarm)
        {
            _audioSource.Stop();
            _isAudioPlaying = false;
        }

        _playerHud?.SetFillRatio(ratio);

        UpdateSpriteOrientation();
    }

    void UpdateSpriteOrientation()
    {
        if (transform.position.x == _lastPositionX)
        {
            return;
        }

        float newYRotation = 0;

        if (_wholeBodyController.transform.position.x < _lastPositionX)
        {
            newYRotation = 180;
        }

        Vector3 newRotation = new Vector3(0f, newYRotation, 0f);

        _wholeBodyController.transform.localEulerAngles = newRotation;
        _lastPositionX = transform.position.x;
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
                Plant plant = element.GetComponent<Plant>();
                if (plant != null)
                {
                    if (plant.AddWater((Water)_objectHolder.GetCurrentTransportableElement()))
                    {
                        break;
                    }
                }

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
            foreach (GameObject element in _interactablesElement)
            {
                IInteractable interactable = element.GetComponent<IInteractable>();
                Linker linker = element.GetComponent<Linker>();

                // if it's a linker try to take the object from it
                if (linker != null)
                {
                    ObjectHolder holder = linker.GetComponent<ObjectHolder>();
                    if (holder != null && holder.HasObject() && _objectHolder != null && !_objectHolder.HasObject())
                    {
                        _objectHolder.TakeFrom(holder);
                        break;
                    }
                }
                else if (interactable != null && interactable.CanInteract())
                {
                    interactable.DoInteraction(this);
                    break;
                }
            }
        }
    }

    public void OnMainAction(InputValue value)
    {
        if (_craftStation != null && !_objectHolder.HasObject())
        {
            if (value.isPressed)
            {
                _craftStation.StartCraft(this);
            }
            else
            {
                _craftStation.StopCraft(this);
            }
        }
    }

    public void OnSelectNextBlueprint(InputValue value)
    {
        _craftStation?.NextBlueprint();
    }

    public void OnSelectPrevBlueprint(InputValue value)
    {
        _craftStation?.PrevBlueprint();
    }

#endregion

#region Linking Logic

    public void OnUnlink(Linker owner)
    {
        Assert.IsTrue(IsLinked());
        _linker = null;
        _oxygenComponent.Plugged = false;
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

        _bodyRenderer.enabled = false;
        _mainRenderer.enabled = false;
        _secondaryRenderer.enabled = false;
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

        CraftStation craftStation = collision.gameObject.GetComponent<CraftStation>();
        if (craftStation != null)
        {
            _craftStation = craftStation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();
        if (newInteractable != null)
        {
            _interactablesElement.Remove(collision.gameObject);
        }

        CraftStation craftStation = collision.gameObject.GetComponent<CraftStation>();
        if (_craftStation == craftStation)
        {
            // abort craft ?
            _craftStation = null;
        }
    }
#endregion

    public void SetColor(ColorCombination color)
    {
        _mainRenderer.color = color.PrimaryColor;
        _secondaryRenderer.color = color.SecondaryColor;
    }

}

