using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ILinkable
{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private float _interactionDistance = 1f;

    private Vector3 _direction = Vector3.zero;
    private Linker _linker = null;

    private ATransportableElement _currentElementInPossession = null;

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.Translate(_speed * Time.deltaTime * _direction);
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        _direction = new Vector3(move.x, move.y, 0f);
    }

    public void OnMainAction(InputValue value)
    {
        if (IsLinked())
        {
            _linker.RemoveLink(this);
            _linker = null;
        }
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
            {
                nearestLinker.DoInteraction(this);
                _linker = nearestLinker;
            }
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsLinked()
    {
        return _linker != null;
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
}

