using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATransportableElement : MonoBehaviour, IInteractable
{
    static protected Transform _levelLayout;
    ObjectHolder _holder = null;
    Rigidbody2D _rigidBody = null;

    protected AudioSource _audioSource;

    #region IInteractable
    public virtual void DoInteraction(Player player)
    {
        ObjectHolder holder = player.GetComponent<ObjectHolder>();
        if (holder != null)
        {
            player.GetComponent<ObjectHolder>().Take(this);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
#endregion

    protected virtual void Awake()
    {
        _rigidBody = GetComponentInChildren<Rigidbody2D>();
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 0.5f;

        if (_levelLayout == null)
        {
            _levelLayout = FindObjectOfType<LevelLayoutHook>().transform;
        }

        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public virtual void Take(ObjectHolder holder)
    {
        _audioSource.clip = ManagersManager.Instance.Get<SoundManager>().GetAudioClip(SoundName.ObjectPickup);
        _audioSource.Play();

        if (_holder != null)
        {
            _holder.RemoveTransportableElement();
        }
        _holder = holder;
        this.transform.SetParent(holder.transform);
        _rigidBody.simulated = false;
    }

    public virtual void Release()
    {
        _audioSource.clip = ManagersManager.Instance.Get<SoundManager>().GetAudioClip(SoundName.ObjectDrop);
        _audioSource.Play();

        _holder = null;
        this.transform.SetParent(_levelLayout);
        _rigidBody.simulated = true;
    }

    public bool IsCarried()
    {
        return _holder != null;
    }
}
