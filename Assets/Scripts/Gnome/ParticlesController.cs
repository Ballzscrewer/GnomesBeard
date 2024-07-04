using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : SFXController
{
    [SerializeField] private ParticleSystem _particleSystem;
    private bool _shiftState = false;
    private bool _movementState = false;


    protected override void OnAwaking() { }

    protected override void OnOnEnabling() { }

    protected override void MovingReaction(bool isMoving)
    {
        _movementState = isMoving;
        SetEmission();
    }

    protected override void ShiftReaction(bool shiftSate)
    {
        _shiftState = shiftSate;
        SetEmission();
    }

    private void SetEmission()
    {
        if (_particleSystem != null)
        {
            var emissionModule = _particleSystem.emission;
            emissionModule.enabled = (_movementState && _shiftState);
        }
    }
}
