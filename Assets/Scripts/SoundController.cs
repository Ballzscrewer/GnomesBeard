using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : SFXController
{
    [SerializeField] private AudioClip _footsStepping;
    private AudioSource _audioSource;

    protected override void OnAwaking()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnOnEnabling()
    {
        _audioSource.clip = _footsStepping;
        _audioSource.pitch = 1f;
    }

    protected override void MovingReaction(bool isMoving)
    {
        if (isMoving) _audioSource.Play();
        else _audioSource.Stop();
    }

    protected override void ShiftReaction(bool shifted)
    {
        if (shifted) _audioSource.pitch = 0.6f / 0.333f;
        else _audioSource.pitch = 1f;
    }
}

