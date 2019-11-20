using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [SerializeField]
    private AudioClip _audioClip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        _audioSource.loop = true;
        _audioSource.Play();

        SceneManager.LoadScene(1);
    }

}
