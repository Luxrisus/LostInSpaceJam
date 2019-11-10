using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _onWin = null;
    [SerializeField]
    private GameObject _onLoose = null;

    void Start()
    {
        if (PlayerPrefs.GetInt("isWin", 0) != 0)
        {
            _onWin.SetActive(true);
        }
        else
        {
            _onLoose.SetActive(true);
        }
    }

    public void OnSubmit()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
