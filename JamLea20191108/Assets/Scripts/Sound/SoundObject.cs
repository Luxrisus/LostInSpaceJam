using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Bank", menuName = "Sound Bank/Sound", order = 1)]
public class SoundObject : ScriptableObject
{
    public SoundName SoundName;
    public AudioClip Clip;
}
