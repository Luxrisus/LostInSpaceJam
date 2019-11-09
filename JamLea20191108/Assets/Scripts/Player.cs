using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ILinkable
{
    [SerializeField]
    private float _speed = 5f;

    Vector3 _direction = Vector3.zero;

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
        // Detect if there is something interactable near the player
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
