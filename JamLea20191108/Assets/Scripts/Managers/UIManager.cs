using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : AManager
{
    public GameObject DefeatGameScreenPrefab;
    private GameObject DefeatGameScreen;

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void DisplayEndOfLevelScreen(bool isWin)
    {
        DefeatGameScreen = Instantiate(DefeatGameScreenPrefab);
    }
}
