using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
    WaitingForBlackhole = 0,
    LevelScrolling,
    EndOfGame
}

public class LevelManager : AManager
{
    [SerializeField]
    private float _timeBeforeBlackHole = 1f;

    [SerializeField]
    private float _levelScrollingSpeed = 1f;

    private float _timeSinceBeginningOfLevel;

    private Blackhole _blackhole;
    private GameObject _levelLayout;
    private LevelState _levelState;

    public override void Initialize()
    {
        _timeSinceBeginningOfLevel = 0f;
        _levelState = LevelState.WaitingForBlackhole;

        _blackhole = FindObjectOfType<Blackhole>();
        var levelLayout = FindObjectOfType<LevelLayoutHook>();
        _levelLayout = levelLayout.gameObject;

        _timeBeforeBlackHole = levelLayout.TimeBeforeBlackHole;
        _levelScrollingSpeed = levelLayout.SideScrollingSpeed;

        if(_blackhole!=null)
        {
            _blackhole.gameObject.SetActive(false);
        }

    }

    void Update()
    {
        switch (_levelState)
        {
            case LevelState.WaitingForBlackhole:
                UpdateTimeBeforeBlackhole();
                break;

            case LevelState.LevelScrolling:
                MoveLevel();
                break;

            default:
                break;
        }
    }

    private void UpdateTimeBeforeBlackhole()
    {
        _timeSinceBeginningOfLevel += Time.deltaTime;

        if(_timeSinceBeginningOfLevel > _timeBeforeBlackHole)
        {
            _blackhole.gameObject.SetActive(true);
            _levelState = LevelState.LevelScrolling;
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
        _levelState = LevelState.EndOfGame;
        _levelScrollingSpeed = 0f;
        ManagersManager.Instance.Get<UIManager>().DisplayEndOfLevelScreen(isWin);
    }

    public LevelState getLevelState()
    {
        return _levelState;
    }
}
