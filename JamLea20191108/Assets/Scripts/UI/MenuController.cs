using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    List<MenuEntry> _entries = new List<MenuEntry>();
    int _index = 0;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            MenuEntry entry = transform.GetChild(i).gameObject.GetComponent<MenuEntry>();
            if (entry != null)
            {
                _entries.Add(entry);
            }
        }
        _entries[_index].Select();
    }

    public void OnNavigate(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        Debug.Log("value" + direction);

        int prevIndex = _index;
        _index -= (int)direction.y;
        if (_index >= _entries.Count)
        {
            _index = 0;
        }
        else if (_index < 0)
        {
            _index = _entries.Count - 1;
        }
        
        _entries[prevIndex].Unselect();
        _entries[_index].Select();
    }

    public void OnSubmit()
    {
        _entries[_index].DoAction();
    }
}
