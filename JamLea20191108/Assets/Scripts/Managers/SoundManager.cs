using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : AManager
{
    [SerializeField]
    private List<SoundObject> _sounds = new List<SoundObject>();

    private Dictionary<SoundName, SoundObject> _soundDic = new Dictionary<SoundName, SoundObject>();

    public override void Initialize()
    {
        foreach(var sound in _sounds)
        {
            _soundDic.Add(sound.SoundName, sound);
        }

        _sounds.Clear();
    }

    public SoundObject GetSoundObject(SoundName soundName)
    {
        return _soundDic[soundName];
    }

    public AudioClip GetAudioClip(SoundName soundName)
    {
        return GetSoundObject(soundName).Clip;
    }
}
