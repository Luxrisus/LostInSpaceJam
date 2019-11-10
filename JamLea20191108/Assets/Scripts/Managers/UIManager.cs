using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : AManager
{
    public GameObject DefeatGameScreenPrefab;
    private GameObject DefeatGameScreen;

    [SerializeField]
    PlayersPanel _playersPanelPrefab;

    private PlayersPanel _playersPanel;

    public PlayersPanel PlayersPanel { get { return _playersPanel; } set { _playersPanel = value; } }

    private Canvas _canvas;

    public override void Initialize()
    {
        _canvas = FindObjectOfType<Canvas>();

        PlayersPanel = Instantiate(_playersPanelPrefab, _canvas.transform);
    }

    public void DisplayEndOfLevelScreen(bool isWin)
    {
        DefeatGameScreen = Instantiate(DefeatGameScreenPrefab);
    }
}
