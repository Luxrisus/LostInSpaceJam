using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : AManager
{
    public GameObject WinGameScreenPrefab;
    private GameObject WinGameScreen;

    public GameObject DefeatGameScreenPrefab;
    private GameObject DefeatGameScreen;

    public PlayersPanel PlayersPanelPrefab = null;
    private PlayersPanel _playersPanel;

    public PlayersPanel PlayersPanel { get { return _playersPanel; } set { _playersPanel = value; } }

    public override void Initialize()
    {
        PlayersPanel = Instantiate(PlayersPanelPrefab);
    }

    public void DisplayEndOfLevelScreen(bool isWin)
    {
        if(isWin == true)
        {
            WinGameScreen = Instantiate(WinGameScreenPrefab);
        }
        else
        {
            DefeatGameScreen = Instantiate(DefeatGameScreenPrefab);
        }
    }
}
