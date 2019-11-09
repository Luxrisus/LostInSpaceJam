using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    Vector3 _direction = Vector3.zero;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.rightTrigger.wasPressedThisFrame)
            {
                Debug.Log("Fire test");
            }

            transform.Translate(_speed * Time.deltaTime * _direction);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        _direction = new Vector3(move.x, move.y, 0f);
    }
}
