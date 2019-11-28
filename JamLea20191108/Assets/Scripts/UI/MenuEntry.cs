using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntry : MonoBehaviour
{
    [SerializeField]
    private Color _selectedColor = Color.red;

    [SerializeField]
    private string _sceneName = "";

    private Color _currentColor;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Select()
    {
        _currentColor = GetComponent<UnityEngine.UI.Image>().color;
        GetComponent<UnityEngine.UI.Image>().color = _selectedColor;
    }

    public void Unselect()
    {
        GetComponent<UnityEngine.UI.Image>().color = _currentColor;
    }

    public void DoAction()
    {
        _audioSource?.Play();
        StartCoroutine(CoroutineDoAction());
    }

    IEnumerator CoroutineDoAction()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
