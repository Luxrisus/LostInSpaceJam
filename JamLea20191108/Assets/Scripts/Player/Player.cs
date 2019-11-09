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

    [SerializeField]
    private float _interactionDistance = 1f;

    private Vector3 _direction = Vector3.zero;
    private Linker _linker = null;

    private ATransportableElement _currentElementInPossession = null;

    [SerializeField]
    private OxygenComponent _oxygenComponent = null;

    private List<IInteractable> _interactablesElement = new List<IInteractable>();
#endregion

    void Start()
    {
        Assert.IsNotNull(_oxygenComponent);
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
            Die();
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        _direction = new Vector3(move.x, move.y, 0f);
    }

    public void OnMainAction(InputValue value)
    {
        foreach(var element in _interactablesElement)
        {
            element.DoInteraction(this);
        }

        if (IsLinked())
            Unlink();
        else
        {
            Linker[] linkers = FindObjectsOfType<Linker>();

            float nearest = float.MaxValue;
            Linker nearestLinker = null;
            foreach (Linker linker in linkers)
            {
                float distance = Vector3.Distance(linker.GetPosition(), GetPosition());

                if (distance < nearest)
                {
                    nearest = distance;
                    nearestLinker = linker;
                }
                nearest = Mathf.Min(nearest, distance);
            }

            if (nearest < _interactionDistance && nearestLinker != null)
                Link(nearestLinker);
        }
    }

#region Linking Logic

    public void Unlink()
    {
        Assert.IsTrue(IsLinked());
        _linker.RemoveLink(this);
        _linker = null;
        _oxygenComponent.Plugged = false;
        Debug.Log("Houston, this is Jacky ! Holding my respiration ! Ovaire !");
    }

    public void Link(Linker linker)
    {
        linker.DoInteraction(this);
        _linker = linker;
        _oxygenComponent.Plugged = true;
        Debug.Log("Houston, this is Jacky ! I peux respirer now. Ovaire !");
    }

    public bool IsLinked()
    {
        return _linker != null;
    }
 #endregion

    public void Die()
    {
        if (IsLinked())
            Unlink();
        Debug.Log("Houston ! I got un problème with the respiration ! AAraaarraAAaaaargh...");
        Destroy(gameObject);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Take(ATransportableElement element)
    {
        _currentElementInPossession = element;
    }

    public ATransportableElement GetCurrentTransportableElement()
    {
        return _currentElementInPossession;
    }

    public void RemoveTransportableElement()
    {
        _currentElementInPossession = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();
        if (newInteractable != null)
        {
            _interactablesElement.Add(newInteractable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.gameObject.GetComponent<IInteractable>();
        if (newInteractable != null)
        {
            _interactablesElement.Remove(newInteractable);
        }
    }
}

