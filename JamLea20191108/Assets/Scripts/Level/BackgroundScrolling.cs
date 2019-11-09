using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField]
    private float _scrollingSpeedX = 0.5f;

    [SerializeField]
    private float _scrollingYDuration = 5f;

    [SerializeField]
    private float _yScrollingMargin = 6f;

    [SerializeField]
    private float _LeftThreshold = -65f;

    [SerializeField]
    private List<GameObject> _boards = new List<GameObject>();

    private const float _sizeOfElements = 40.9f;

    private Queue<GameObject> _parralaxEffect = new Queue<GameObject>();
    private bool _isAscending = true;
    private float currentYAscendingTime = 0f;

    private void Awake()
    {
        int index = 0;
        foreach(var board in _boards)
        {
            Vector3 position = board.transform.localPosition;
            position.x = index * _sizeOfElements;
            board.transform.localPosition = position;

            _parralaxEffect.Enqueue(board);
            ++index;
        }

        currentYAscendingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float ascendingFactor = _isAscending ? 1 : -1;
        bool hasReachedLeftLimit = false;
        float highestPosition = 0f;

        foreach (var board in _boards)
        {
            Vector3 localPosition = board.transform.localPosition;
            float xPosition = localPosition.x;
            float yPosition = localPosition.y;
            float minimumY = _isAscending ? _yScrollingMargin : -_yScrollingMargin;
            float maximumY = -minimumY;

            float t = (Time.time - currentYAscendingTime) / _scrollingYDuration;
            float targetX = Mathf.Lerp(xPosition, xPosition - _scrollingSpeedX, Time.deltaTime);
            float targetY = Mathf.SmoothStep(minimumY, maximumY, t);

            board.transform.localPosition = new Vector3(targetX, targetY, localPosition.z);

            if (targetX < _LeftThreshold)
            {
                hasReachedLeftLimit = true;
            }

            highestPosition = highestPosition < targetX ? targetX : highestPosition;

           CheckForAscending(targetY);
        }

        if (hasReachedLeftLimit)
        {
            CheckForNewPosition(highestPosition);
        }
    }

    private void CheckForAscending(float targetY)
    {
        if (targetY <= -_yScrollingMargin && _isAscending)
        {
            _isAscending = false;
            currentYAscendingTime = Time.time;
        }
        else if (targetY >= _yScrollingMargin && !_isAscending)
        {
            _isAscending = true;
            currentYAscendingTime = Time.time;
        }
    }

    private void CheckForNewPosition(float highestCurrentPosition)
    {
        GameObject leftElement = _parralaxEffect.Dequeue();
        Vector3 position = leftElement.transform.localPosition;

        leftElement.transform.localPosition = new Vector3(highestCurrentPosition + _sizeOfElements, position.y, position.z);
        _parralaxEffect.Enqueue(leftElement);
    }
}
