using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : AManager
{
    [SerializeField]
    private float _timeBeforeBlackHole;

    private float _timeSinceBeginningOfLevel;

    private Blackhole _blackhole;
    private bool _isBlackHoleOpened = false;

    public override void Initialize()
    {
        _timeSinceBeginningOfLevel = 0f;
    }

    private void Start()
    {
        _blackhole = FindObjectOfType<Blackhole>();
        if(_blackhole!=null)
        {
            _blackhole.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        _timeSinceBeginningOfLevel += Time.deltaTime;

        if(_timeSinceBeginningOfLevel > _timeBeforeBlackHole && !_isBlackHoleOpened)
        {
            _blackhole.gameObject.SetActive(true);
            _isBlackHoleOpened = true;
        }
    }

    public void EndOfLevel(bool isWin)
    {
        ManagersManager.Instance.Get<UIManager>().DisplayEndOfLevelScreen(isWin);
    }
}
