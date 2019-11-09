using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        UpdatePosition(Vector3.zero, Vector3.zero);
    }
    
    public void SetStart(Vector2 start)
    {
        _lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0f));
    }
    
    public void SetEnd(Vector2 end)
    {
        _lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0f));
    }

    public void UpdatePosition(Vector2 start, Vector2 end)
    {
        if (_lineRenderer != null)
        {
            SetStart(start);
            SetEnd(end);
        }
    }

    public float GetDistance()
    {
        if (_lineRenderer != null)
        {
            return Vector3.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));
        }
        return 0f;
    }
}
