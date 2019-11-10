using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : AManager
{
    [SerializeField]
    private float _timeBeforeBlackHole = 1f;

    [SerializeField]
    private float _levelScrollingSpeed = 1f;

    private float _timeSinceBeginningOfLevel;

    private Blackhole _blackhole;
    private bool _isBlackHoleOpened = false;
    private GameObject _levelLayout;

    public override void Initialize()
    {
        _timeSinceBeginningOfLevel = 0f;

        _blackhole = FindObjectOfType<Blackhole>();
        _levelLayout = FindObjectOfType<LevelLayoutHook>().gameObject;

        if(_blackhole!=null)
        {
            _blackhole.gameObject.SetActive(false);
        }

    }

    void Update()
    {
        if (!_isBlackHoleOpened)
        {
            UpdateTimeBeforeBlackhole();
        }
        else
        {
            MoveLevel();
        }
    }

    private void UpdateTimeBeforeBlackhole()
    {
        _timeSinceBeginningOfLevel += Time.deltaTime;

        if(_timeSinceBeginningOfLevel > _timeBeforeBlackHole && !_isBlackHoleOpened)
        {
            _blackhole.gameObject.SetActive(true);
            _isBlackHoleOpened = true;
        }
    }

    private void MoveLevel()
    {
        Vector3 position = _levelLayout.transform.localPosition;

        position.x = Mathf.Lerp(position.x, position.x - _levelScrollingSpeed, Time.deltaTime);

        _levelLayout.transform.localPosition = position;
    }

    public void EndOfLevel(bool isWin)
    {
        ManagersManager.Instance.Get<UIManager>().DisplayEndOfLevelScreen(isWin);
    }
}
